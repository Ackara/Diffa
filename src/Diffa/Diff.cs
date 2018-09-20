using Acklann.Diffa.Exceptions;
using Acklann.Diffa.Reporters;
using Acklann.Diffa.Resolution;
using System;
using System.IO;
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
            foreach (string variable in new[] { "DISABLE_DIFFA_REPORTERS" })
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
        /// <param name="fileExtension">The file extension (with or without a dot).</param>
        /// <param name="reporter">The reporter.</param>
        /// <param name="fileResolver">The file resolver.</param>
        ///
        public static void Approve(IApprover approver, object subject, object[] testParameters, string fileExtension = ".txt", IReporter reporter = default, IFileResolver fileResolver = default)
        {
            var contextBuilder = new StackTraceParser();
            if (fileResolver == null) fileResolver = new ContextualFileResolver(contextBuilder);

            string resultFile = fileResolver.GetResultFilePath(fileExtension, testParameters);
            string approvedFile = fileResolver.GetApprovedFilePath(fileExtension, testParameters);

            if (approver.Approve(subject, resultFile, approvedFile, out string reasonWhyItWasNotApproved) == false)
            {
                if (_shouldReport ?? true)
                {
                    if (reporter == null)
                    {
                        UseAttribute attribute = contextBuilder.Context.ReporterAttribute;
                        if (attribute?.Reporter == null)
                            reporter = _reporterFactory.GetFirstAvailableReporter(true);
                        else
                            reporter = (IReporter)Activator.CreateInstance(attribute.Reporter, args: attribute.ShouldPause);
                    }

                    if (reporter.Launch(resultFile, approvedFile))
                    {
                        // Checking the results again because the user may have updated the approved file if a diff reporter was launched.
                        if (approver.Approve(subject, resultFile, approvedFile, out reasonWhyItWasNotApproved)) return;
                    }
                }

                throw new ResultNotApprovedException(ExceptionMessage.GetResultWasNotApproved(resultFile, approvedFile, reasonWhyItWasNotApproved));
            }
        }

        /* ========== */

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file.
        /// </summary>
        /// <param name="subject">The subject/test result.</param>
        /// <param name="fileExtension">The file extension (with or without a dot).</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void Approve(object subject, string fileExtension = ".txt", params object[] args)
        {
            Approve(new BinaryApprover(), Encoding.Default.GetBytes(subject.ToString()), args, fileExtension);
        }

        /// <summary>
        /// Assert that the <paramref name="resultFilePath"/> is equal to it's approved file.
        /// </summary>
        /// <param name="resultFilePath">The file path.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ApproveFile(string resultFilePath, params object[] args)
        {
            Approve(new BinaryApprover(), File.ReadAllBytes(resultFilePath), args, Path.GetExtension(resultFilePath));
        }

        /// <summary>
        /// Assert that the document conforms to the specified XML-Schema (.xsd).
        /// </summary>
        /// <param name="xml">The XML stream.</param>
        /// <param name="schemaFilePath">The URI that specifies the schema to load.</param>
        /// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace specified in the schema.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ApproveXml(Stream xml, string schemaFilePath, string targetNamespace, params object[] args)
        {
            if (File.Exists(schemaFilePath))
            {
                Approve(new XmlApprover(schemaFilePath, targetNamespace), xml, args, ".xml");
            }
            else throw new FileNotFoundException($"Could not find file '{schemaFilePath}'.", schemaFilePath);
        }

        /// <summary>
        /// Assert that the document conforms to the specified XML-Schema (.xsd).
        /// </summary>
        /// <param name="xml">The XML text.</param>
        /// <param name="schemaFilePath">The schema file path.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ApproveXml(string xml, string schemaFilePath, string targetNamespace, params object[] args)
        {
            if (File.Exists(schemaFilePath))
            {
                Approve(new XmlApprover(schemaFilePath, targetNamespace), new MemoryStream(Encoding.UTF8.GetBytes(xml)), args, ".xml");
            }
            else throw new FileNotFoundException($"Could not find file '{schemaFilePath}'.", schemaFilePath);
        }

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file.
        /// </summary>
        /// <param name="subject">The subject/test result.</param>
        /// <param name="fileExtension">The file extension (with or without a dot).</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ShouldMatchApprovedFile(this object subject, string fileExtension = ".txt", params object[] args)
        {
            Approve(new BinaryApprover(), Encoding.Default.GetBytes(subject.ToString()), args, fileExtension);
        }

        #region Private Members

        private static readonly bool? _shouldReport;
        private static readonly ReporterFactory _reporterFactory = new ReporterFactory();

        #endregion Private Members
    }
}