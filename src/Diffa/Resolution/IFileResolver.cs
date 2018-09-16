namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Defines methods to retrieve the full path of a result and approved file.
    /// </summary>
    public interface IFileResolver
    {
        /// <summary>
        /// Gets the result file path.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="args">The parameterized test arguments.</param>
        /// <returns></returns>
        string GetResultFilePath(string fileExtension, params object[] args);

        /// <summary>
        /// Gets the approved file path.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="args">The parameterized test arguments.</param>
        /// <returns></returns>
        string GetApprovedFilePath(string fileExtension, params object[] args);
    }
}