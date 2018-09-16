using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Performs a byte-to-byte comparison between two .xml files or an .xsd file.
    /// </summary>
    /// <seealso cref="Resolution.ApproverBase{T}" />
    public class XmlApprover : ApproverBase<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlApprover" /> class.
        /// </summary>
        /// <param name="schemaUri">The URI that specifies the schema to load.</param>
        /// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace specified in the schema.</param>
        /// <param name="failOnWarnings">if set to <c>true</c> [throw on warnings].</param>
        public XmlApprover(string schemaUri, string targetNamespace, bool failOnWarnings = false)
        {
            _schema = new XmlSchemaSet();
            _failOnWarnings = failOnWarnings;
            _schema.Add(targetNamespace, schemaUri);
        }

        /// <summary>
        /// Asserts that the serialized <paramref name="subject" /> equals the <paramref name="approvedFilePath" /> contents.
        /// </summary>
        /// <param name="subject">The test subject.</param>
        /// <param name="resultFilePath">The test result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <param name="reasonWhyItWasNotApproved">The reason why <paramref name="subject" /> was not approved.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool Approve(Stream subject, string resultFilePath, string approvedFilePath, out string reasonWhyItWasNotApproved)
        {
            bool approved;
            int warns = 0, errors = 0;
            var errorList = new StringBuilder();

            var doc = XDocument.Load(subject);
            subject.Position = 0;
            
            doc.Validate(_schema, delegate (object sender, ValidationEventArgs e)
            {
                if (e.Severity == XmlSeverityType.Error) errors++;
                else if (e.Severity == XmlSeverityType.Warning) warns++;

                errorList.AppendLine($"[{e.Severity}]  {e.Message}");
                errorList.AppendLine();
            });
            reasonWhyItWasNotApproved = errorList.ToString();

            approved = (_failOnWarnings ? (warns == 0 && errors == 0) : errors == 0);
            if (approved == false)
            {
                CreateFileIfNotExist(resultFilePath);
                using (var file = new FileStream(resultFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine($"ERRORS:");
                    writer.WriteLine(errorList);
                    writer.WriteLine();

                    writer.WriteLine($"DOCUMENT:");
                    writer.WriteLine();
                    writer.WriteLine(doc.ToString());
                    writer.Flush();
                }
            }

            return approved;
        }

        #region Private Members

        private readonly XmlSchemaSet _schema;
        private readonly bool _failOnWarnings;

        #endregion Private Members
    }
}