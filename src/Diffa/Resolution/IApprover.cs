namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Defines a method to determine if a test result is valid.
    /// </summary>
    public interface IApprover
    {
        /// <summary>
        /// Asserts that the serialized <paramref name="subject"/> equals the <paramref name="approvedFilePath"/> contents.
        /// </summary>
        /// <param name="subject">The test result object.</param>
        /// <param name="resultFilePath">The test result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <param name="reasonWhyItWasNotApproved">The reason why the <paramref name="subject"/> was not approved.</param>
        /// <returns></returns>
        bool Approve(object subject, string resultFilePath, string approvedFilePath, out string reasonWhyItWasNotApproved);
    }
}