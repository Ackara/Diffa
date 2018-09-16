namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Represents a null <see cref="IReporter"/>.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.IReporter" />
    public class NullReporter : IReporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullReporter"/> class.
        /// </summary>
        public NullReporter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullReporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> [should pause].</param>
        public NullReporter(bool shouldPause)
        {
        }

        /// <summary>
        /// Returns true.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this reporter is installed; otherwise, <c>false</c>.
        /// </returns>
        public bool CanLaunch() => true;

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="resultFilePath">The result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        public void Launch(string resultFilePath, string approvedFilePath)
        {
        }
    }
}