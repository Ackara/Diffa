using Acklann.Diffa.Exceptions;
using Acklann.Diffa.Reporters;
using Acklann.Diffa.Resolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
[assembly: System.CLSCompliant(true)]
namespace Acklann.Diffa
{
    /// <summary>
    /// A collection of methods to compare an object against an approved file. If the condition being tested is not met, a diff tool will be launched so you may compare the difference.
    /// </summary>
    public static class Diff
    {
        static Diff()
        {
            try
            {
                string disable = Environment.GetEnvironmentVariable("%DISABLE_DIFFA_REPORTERS%", EnvironmentVariableTarget.Process | EnvironmentVariableTarget.User);
                if (string.IsNullOrEmpty(disable))
                    _shouldReport = true;
                else
                    bool.TryParse(disable, out _shouldReport);

                foreach (string name in new[] { "%MSBuildProjectDirectory%", "%PROJECT_DIRECTORY%" })
                    if (string.IsNullOrEmpty(TestContext.ProjectDirectory))
                    {
                        TestContext.ProjectDirectory = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process | EnvironmentVariableTarget.User);
                    }
                    else break;
            }
            catch { }
        }

        /// <summary>
        /// Assert that the serialized <paramref name="subject" /> is equal to it's approved file.
        /// </summary>
        /// <param name="approver">The approver.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="testParameters">The test parameters supplied by parameterized test.</param>
        /// <param name="fileExtension">The file extension (with or without a dot).</param>
        /// <param name="reporter">The reporter.</param>
        /// <param name="fileResolver">The file resolver.</param>
        /// <exception cref="Acklann.Diffa.Exceptions.ResultNotApprovedException">The subject did not match the approved-file.</exception>
        public static void Approve(IApprover approver, object subject, object[] testParameters, string fileExtension = ".txt", IReporter reporter = default, IFileResolver fileResolver = default)
        {
            var contextBuilder = new StackTraceParser();
            if (fileResolver == null) fileResolver = new ContextualFileResolver(contextBuilder);

            string resultFile = fileResolver.GetResultFilePath(fileExtension, testParameters);
            string approvedFile = fileResolver.GetApprovedFilePath(fileExtension, testParameters);

            if (approver.Approve(subject, resultFile, approvedFile, out string reasonWhyItWasNotApproved) == false)
            {
                if (_shouldReport)
                {
                    if (reporter == null)
                    {
                        ReporterAttribute attribute = contextBuilder.Context.ReporterAttribute;
                        if (attribute?.Reporter == null)
                            reporter = _reporterFactory.GetFirstAvailableReporter(attribute.ShouldInterrupt);
                        else
                            reporter = (IReporter)Activator.CreateInstance(attribute.Reporter, args: attribute.ShouldInterrupt);
                    }

                    if (reporter.Launch(resultFile, approvedFile))
                    {
                        // Checking the results again because the user may have updated the approved file when the reporter was launched.
                        if (approver.Approve(subject, resultFile, approvedFile, out reasonWhyItWasNotApproved)) return;
                    }
                }

                throw new ResultNotApprovedException(ExceptionMessage.GetResultWasNotApproved(resultFile, approvedFile, reasonWhyItWasNotApproved));
            }
        }

        // ========== //

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
        /// Assert that the JSON text is equal to it's approved file.
        /// </summary>
        /// <param name="json">The json text.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ApproveJson(string json, params object[] args)
        {
            Approve(new BinaryApprover(), Encoding.UTF8.GetBytes(json), args, ".json");
        }

        /// <summary>
        /// Assert that the serialized <paramref name="subject"/> is equal to it's approved file.
        /// </summary>
        /// <param name="subject">The subject/test result.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ApproveJson(object subject, params object[] args)
        {
            byte[] data = null;

            using (var stream = new MemoryStream())
            using (var writer = System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, false, indent: true))
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(subject.GetType(), new System.Runtime.Serialization.Json.DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true, DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("yyyy-MM-dd hh:mm:ss.fff tt") });

                serializer.WriteObject(writer, subject);
                writer.Flush();
                data = stream.ToArray();
            }

            Approve(new BinaryApprover(), data, args, ".json");
        }

        /// <summary>
        /// Assert that the document conforms to the specified XML-Schema (.xsd).
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="schemaFilePath">The schema file path.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        /// <exception cref="FileNotFoundException">Could not find XML-Schema (.xsd).</exception>
        public static void ApproveXml(object subject, string schemaFilePath, string targetNamespace, params object[] args)
        {
            if (File.Exists(schemaFilePath))
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(subject.GetType());
                    serializer.Serialize(stream, subject, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, targetNamespace) }));
                    stream.Position = 0;

                    Approve(new XmlApprover(schemaFilePath, targetNamespace), stream, args, ".xml");
                }
            }
            else throw new FileNotFoundException($"Could not find XML-Schema (.xsd) at '{schemaFilePath}'", schemaFilePath);
        }

        /// <summary>
        /// Assert that the document conforms to the specified XML-Schema (.xsd).
        /// </summary>
        /// <param name="xml">The XML stream.</param>
        /// <param name="schemaFilePath">The URI that specifies the schema to load.</param>
        /// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace specified in the schema.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        /// <exception cref="FileNotFoundException">Could not find XML-Schema (.xsd).</exception>
        public static void ApproveXml(Stream xml, string schemaFilePath, string targetNamespace, params object[] args)
        {
            if (File.Exists(schemaFilePath))
            {
                Approve(new XmlApprover(schemaFilePath, targetNamespace), xml, args, ".xml");
            }
            else throw new FileNotFoundException($"Could not find XML-Schema (.xsd) at '{schemaFilePath}'.", schemaFilePath);
        }

        /// <summary>
        /// Assert that the document conforms to the specified XML-Schema (.xsd).
        /// </summary>
        /// <param name="xml">The XML text.</param>
        /// <param name="schemaFilePath">The schema file path.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        /// <exception cref="FileNotFoundException">Could not find XML-Schema (.xsd).</exception>
        public static void ApproveXml(string xml, string schemaFilePath, string targetNamespace, params object[] args)
        {
            if (File.Exists(schemaFilePath))
            {
                Approve(new XmlApprover(schemaFilePath, targetNamespace), new MemoryStream(Encoding.UTF8.GetBytes(xml)), args, ".xml");
            }
            else throw new FileNotFoundException($"Could not find XML-Schema (.xsd) at '{schemaFilePath}'.", schemaFilePath);
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

        /// <summary>
        /// Assert that the serialized <paramref name="subjects" /> is equal to it's approved file.
        /// </summary>
        /// <param name="subjects">The subjects.</param>
        /// <param name="fileExtension">The file extension (with or without a dot).</param>
        /// <param name="args">The test parameters supplied by parameterized test.</param>
        public static void ApproveAll(IEnumerable<object> subjects, string fileExtension = ".txt", params object[] args)
        {
            int index = 0;
            var content = new StringBuilder();
            foreach (var item in subjects)
            {
                content.AppendLine($"[{index++}] => {item.ToString()}");
            }

            Approve(new BinaryApprover(), Encoding.Default.GetBytes(content.ToString()), args, fileExtension);
        }

        #region Private Members

        private static readonly bool _shouldReport;
        private static readonly ReporterFactory _reporterFactory = new ReporterFactory();

        #endregion Private Members
    }
}