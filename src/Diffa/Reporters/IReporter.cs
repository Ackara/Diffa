namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Represents a (diff) application that can compare two files.
    /// </summary>
    public interface IReporter
    {
        /// <summary>
        /// Determines whether this reporter is installed on the current machine.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this reporter is installed; otherwise, <c>false</c>.
        /// </returns>
        bool CanLaunch();

        /// <summary>
        /// Launches the reporter application to compare the specified files.
        /// </summary>
        /// <param name="resultFilePath">The result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <returns><c>true</c> if the process waited for the reporter to close; otherwise, <c>false</c></returns>
        bool Launch(string resultFilePath, string approvedFilePath);
    }
}