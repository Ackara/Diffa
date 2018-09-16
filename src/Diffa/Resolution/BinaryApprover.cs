using System.IO;

namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Performs a byte-to-byte comparison between two files.
    /// </summary>
    /// <seealso cref="Resolution.ApproverBase{byte[]}" />
    public class BinaryApprover : ApproverBase<byte[]>
    {
        /// <summary>
        /// Asserts that the serialized <paramref name="subject"/> equals the <paramref name="approvedFilePath"/> contents.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="resultFilePath">The result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <param name="reasonWhyItWasNotApproved">The reason why it was not approved.</param>
        /// <returns></returns>
        public override bool Approve(byte[] fileContents, string resultFilePath, string approvedFilePath, out string reasonWhyItWasNotApproved)
        {
            CreateFileIfNotExist(approvedFilePath);

            if (ByteArrayAreEqual(fileContents, File.ReadAllBytes(approvedFilePath)))
            {
                reasonWhyItWasNotApproved = string.Empty;
                return true;
            }
            else
            {
                CreateFileIfNotExist(resultFilePath);
                File.WriteAllBytes(resultFilePath, fileContents);
                reasonWhyItWasNotApproved = $"The results do not match the approved file '{approvedFilePath}'.";
                return false;
            }
        }
    }
}