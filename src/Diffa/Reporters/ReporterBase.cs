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
        /// <param name="shouldPause">if set to <c>true</c> [should pause].</param>
        public ReporterBase(string executablePath, string argsFormat, bool shouldPause)
        {
            _format = argsFormat;
            _shouldPause = shouldPause;
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
        /// <exception cref="System.Exception"></exception>
        public virtual void Launch(string resultFilePath, string approvedFilePath)
        {
            string args = string.Format(_format, resultFilePath, approvedFilePath);
            try
            {
                using (var exe = new Process())
                {
                    exe.StartInfo.Arguments = args;
                    exe.StartInfo.FileName = _executablePath;

                    exe.Start();
                    if (_shouldPause) exe.WaitForExit();
                }
            }
            catch (System.SystemException ex)
            {
                throw new System.Exception($"Error occurred while not launching {GetType().Name} at {_executablePath} {args}", ex);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _executablePath;
        }

        #region Private Members

        private readonly string _format;
        private readonly string _executablePath;
        private readonly bool _shouldPause;

        #endregion Private Members
    }
}