using System.Diagnostics;
using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Represents a (diff) application that can compare two files.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.IReporter" />
    public abstract class ReporterBase : IReporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReporterBase"/> class.
        /// </summary>
        /// <param name="executablePath">The executable path.</param>
        /// <param name="argsFormat">The arguments format.</param>
        /// <param name="shouldInterrupt">if set to <c>true</c> [should pause].</param>
        protected ReporterBase(string executablePath, string argsFormat, bool shouldInterrupt)
        {
            _format = argsFormat;
            _shouldInterrupt = shouldInterrupt;
            _executablePath = executablePath;
        }

        /// <summary>
        /// Determines whether this reporter is installed on the current machine.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this reporter is installed; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanLaunch()
        {
            return File.Exists(_executablePath);
        }

        /// <summary>
        /// Launches the reporter application to compare the specified files.
        /// </summary>
        /// <param name="resultFilePath">The result file path.</param>
        /// <param name="approvedFilePath">The approved file path.</param>
        /// <returns><c>true</c> if the process waited for the reporter to close; otherwise, <c>false</c></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public virtual bool Launch(string resultFilePath, string approvedFilePath)
        {
            using (var exe = new Process())
            {
                exe.StartInfo.FileName = _executablePath;
                exe.StartInfo.Arguments = string.Format(_format, resultFilePath, approvedFilePath);

                try
                {
                    exe.Start();
                    if (_shouldInterrupt)
                    {
                        exe.WaitForExit();
                        return true;
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    throw new System.ArgumentException($"Failed to open the {GetType().Name} using the following arguments; filename:'{_executablePath}' arguments:'{exe.StartInfo.Arguments}'", ex);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => _executablePath;

        #region Private Members

        internal readonly string _format, _executablePath;
        private readonly bool _shouldInterrupt;

        #endregion Private Members
    }
}