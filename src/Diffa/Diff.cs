using Acklann.Diffa.Exceptions;
using Acklann.Diffa.Reporters;
using Acklann.Diffa.Resolution;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Acklann.Diffa
{
    /// <summary>
    /// A collection of methods to compare an object against an approved file. If the condition being tested is not met, a diff tool will be launched so you may compare the difference.
    /// </summary>
    public static class Diff
    {
        static Diff()
        {
            foreach (string variable in new[] { "" })
                try
                {
                    if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable($"%{variable}%")) == false)
                    {
                        _shouldReport = false;
                        break;
                    }
                }
                catch (System.Security.SecurityException) { }
        }

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file.
        /// </summary>
        /// <param name="approver">The approver.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="testParameters">The test parameters supplied by parameterized test.</param>
        /// <param name="fileExtension">The file extension (with or without an dot).</param>
        /// <param name="reporter">The reporter.</param>
        /// <param name="fileResolver">The file resolver.</param>
        /// <param name="contextBuilder">The context builder.</param>
        public static void Approve(IApprover approver, object subject, object[] testParameters, string fileExtension = ".txt", IReporter reporter = default, IFileResolver fileResolver = default, IContextBuilder contextBuilder = default)
        {
            if (contextBuilder == null) contextBuilder = new StackTraceContextBuilder();
            if (fileResolver == null) fileResolver = new ContextualFileResolver(contextBuilder);

            string resultFile = fileResolver.GetResultFilePath(fileExtension, testParameters);
            string approvedFile = fileResolver.GetApprovedFilePath(fileExtension, testParameters);

            if (approver.Approve(subject, resultFile, approvedFile, out string reasonWhyItWasNotApproved) == false)
            {
                if (_shouldReport ?? true)
                {
                    bool shouldCheckAgain = false;
                    if (reporter == null)
                    {
                        UseAttribute attribute = contextBuilder.Context.ReporterAttribute;
                        if (attribute?.Reporter == null)
                        {
                            reporter = _reporterFactory.GetFirstAvailableReporter(true);
                        }
                        else
                        {
                            shouldCheckAgain = attribute.ShouldPause;
                            reporter = (IReporter)Activator.CreateInstance(attribute.Reporter, args: attribute.ShouldPause);
                        }
                    }

                    reporter.Launch(resultFile, approvedFile);
                    if (shouldCheckAgain)
                    {
                        if (approver.Approve(subject, resultFile, approvedFile, out reasonWhyItWasNotApproved)) return;
                    }
                }

                throw new ResultNotApprovedException(ExceptionMessage.ResultWasNotApproved(resultFile, approvedFile, reasonWhyItWasNotApproved));
            }
        }

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file. Only use this method with a test method that returns a <see cref="System.Threading.Tasks.Task"/> object.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="fileExtension">The file extension (with or without an dot).</param>
        /// <param name="resultFolder">The result folder.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void ApproveFromAsync(object subject, string fileExtension = ".txt", string resultFolder = "", [CallerFilePath]string sourceFile = null, [CallerMemberName]string methodName = null)
        {
            // TODO: Find a better solution to handle async tests.
            /// The <see cref="StackTraceContextBuilder"/> current implementation is unable to retrieve the source file of the async test methods.
            /// Therefore I am using <see cref="System.Runtime.CompilerServices"/> as a workaround for now. This is a faulty solution because it requires
            /// that this method must be called to directly from the test method to get the its named. I need to find a better solution.

            var builder = new TestContextBuilder()
            {
                SourceFile = sourceFile,
                SubDirectory = resultFolder,
                TestMethodName = methodName,
                TestClassName = Path.GetFileNameWithoutExtension(sourceFile),
            };
            Approve(new BinaryApprover(), Encoding.Default.GetBytes(subject.ToString()), new object[0], fileExtension, null, null, builder);
        }

        /* ========== */

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file.
        /// </summary>
        /// <param name="subject">The subject/test result.</param>
        /// <param name="fileExtension">The file extension (with or without an dot).</param>
        /// <param name="testParameters">The test parameters supplied by parameterized test.</param>
        public static void Approve(object subject, string fileExtension = ".txt", params object[] testParameters)
        {
            Approve(new BinaryApprover(), Encoding.Default.GetBytes(subject.ToString()), testParameters, fileExtension);
        }

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="testParameters">The test parameters supplied by parameterized test.</param>
        public static void ApproveFile(string filePath, params object[] testParameters)
        {
            Approve(new BinaryApprover(), File.ReadAllBytes(filePath), testParameters, Path.GetExtension(filePath));
        }

        #region Private Members

        private static readonly bool? _shouldReport;
        private static readonly ReporterFactory _reporterFactory = new ReporterFactory();

        #endregion Private Members
    }
}