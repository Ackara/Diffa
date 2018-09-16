using System;
using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Represents notepad++.
    /// </summary>
    /// <seealso cref="ReporterBase" />
    public class NotepadPlusPlusReporter : ReporterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadPlusPlusReporter"/> class.
        /// </summary>
        public NotepadPlusPlusReporter() : base(GetExecutablePath(), "{0}", true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadPlusPlusReporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> [should pause].</param>
        public NotepadPlusPlusReporter(bool shouldPause) : base(GetExecutablePath(), "{0}", shouldPause)
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
                        _executablePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Notepad++", "notepad++.exe");
                        break;

                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
                        _executablePath = null;// TODO: Figure out where beyond comparer is installed on mac and Linux.
                        break;
                }

            return _executablePath;
        }
    }
}