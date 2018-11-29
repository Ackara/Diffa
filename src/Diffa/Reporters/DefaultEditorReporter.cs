using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// The operating-system default file editor.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    [Traits(Kind.Editor, Rating.FREE_EDITOR - 1)]
    public class DefaultEditorReporter : ReporterBase
    {
        static DefaultEditorReporter()
        {
            switch (System.Environment.OSVersion.Platform)
            {
                default:
                case System.PlatformID.Win32NT:
                    _exePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "notepad.exe");
                    break;

                case System.PlatformID.Unix:
                case System.PlatformID.MacOSX:
                    _exePath = "/usr/bin/gedit";
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEditorReporter"/> class.
        /// </summary>
        public DefaultEditorReporter() : base(_exePath, "{0}", false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEditorReporter"/> class.
        /// </summary>
        /// <param name="shouldInterrupt">if set to <c>true</c> the test will be paused until the application is closed.</param>
        public DefaultEditorReporter(bool shouldInterrupt) : base(_exePath, "{0}", false)
        { }

        private static readonly string _exePath;
    }
}