using System;
using System.IO;

namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Defines a method to determine if a test result is valid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Acklann.Diffa.Resolution.IApprover" />
    public abstract class ApproverBase<T> : IApprover
    {
        /// <summary>
        /// Asserts that the serialized <paramref name="subject" /> equals the <paramref name="approvedFilePath" /> contents.
        /// </summary>
        /// <param name="subject">The test result object.</param>
        /// <param name="resultFilePath">The test result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <param name="reasonWhyItWasNotApproved">The reason why the <paramref name="subject" /> was not approved.</param>
        /// <returns></returns>
        bool IApprover.Approve(object subject, string resultFilePath, string approvedFilePath, out string reasonWhyItWasNotApproved) => Approve((T)subject, resultFilePath, approvedFilePath, out reasonWhyItWasNotApproved);

        /// <summary>
        /// Asserts that the serialized <paramref name="subject" /> equals the <paramref name="approvedFilePath" /> contents.
        /// </summary>
        /// <param name="subject">The test subject.</param>
        /// <param name="resultFilePath">The test result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <param name="reasonWhyItWasNotApproved">The reason why <paramref name="subject"/> was not approved.</param>
        /// <returns></returns>
        public abstract bool Approve(T subject, string resultFilePath, string approvedFilePath, out string reasonWhyItWasNotApproved);

        /// <summary>
        /// Determines if specified byte arrays are equal.
        /// </summary>
        /// <param name="resultFile">The result file.</param>
        /// <param name="approvedFile">The approved file.</param>
        /// <returns></returns>
        protected static bool ByteArrayAreEqual(ReadOnlySpan<byte> resultFile, ReadOnlySpan<byte> approvedFile)
        {
            return resultFile.SequenceEqual(approvedFile);
        }

        /// <summary>
        /// Creates the file if do not exist.
        /// </summary>
        /// <param name="filePath">The approved file path.</param>
        protected void CreateFileIfNotExist(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                string dir = Path.GetDirectoryName(filePath);
                if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
                File.Create(filePath).Dispose();
            }
        }
    }
}