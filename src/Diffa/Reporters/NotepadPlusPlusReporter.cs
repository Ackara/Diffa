using System;
using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Notepad++ launcher.
    /// </summary>
    /// <seealso cref="ReporterBase" />
    [Traits(Kind.Editor, Rating.FREE_EDITOR)]
    public class NotepadPlusPlusReporter : ReporterBase
    {
        static NotepadPlusPlusReporter()
        {
            switch (Environment.OSVersion.Platform)
            {
                default:
                case PlatformID.Win32NT:
                    _exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Notepad++", "notepad++.exe");
                    break;

                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    // TODO: Figure out where notepad++ is installed on mac and Linux.
                    //_exePath = "/snap/bin/notepad-plus-plus";
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadPlusPlusReporter"/> class.
        /// </summary>
        public NotepadPlusPlusReporter() : base(_exePath, "{0}", false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadPlusPlusReporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> the test will be paused until the application is closed.</param>
        public NotepadPlusPlusReporter(bool shouldPause) : base(_exePath, "{0}", false)
        {
        }

        private static string _exePath;
    }
}