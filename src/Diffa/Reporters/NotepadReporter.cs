using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Notepad launcher.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    [Traits(Kind.Editor, Rating.FREE_EDITOR - 1)]
    public class NotepadReporter : ReporterBase
    {
        static NotepadReporter()
        {
            try { _exePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "notepad.exe"); }
            catch { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadReporter"/> class.
        /// </summary>
        public NotepadReporter() : base(_exePath, "{0}", false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadReporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> the test will be paused until the application is closed.</param>
        public NotepadReporter(bool shouldPause) : base(_exePath, "{0}", false)
        { }

        private static readonly string _exePath;
    }
}