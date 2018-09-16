using System;
using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Represents the Beyond Compare 4 application.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    [Rank(Rating.PAID)]
    public class BeyondCompare4Reporter : ReporterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeyondCompare4Reporter"/> class.
        /// </summary>
        public BeyondCompare4Reporter() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyondCompare4Reporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> [should pause].</param>
        public BeyondCompare4Reporter(bool shouldPause) : base(GetExecutablePath(), "\"{0}\" \"{1}\"", shouldPause)
        {
        }

        private static string _executablePath;

        /// <summary>
        /// Gets the executable path.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutablePath()
        {
            if (string.IsNullOrEmpty(_executablePath))
                switch (Environment.OSVersion.Platform)
                {
                    default:
                    case PlatformID.Win32NT:
                        _executablePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Beyond Compare 4", "BCompare.exe");
                        break;

                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
                        _executablePath = null;// TODO: Figure out where beyond comparer is installed on mac and linux.
                        break;
                }

            return _executablePath;
        }
    }
}