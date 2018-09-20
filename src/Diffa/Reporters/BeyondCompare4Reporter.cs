using System;
using System.IO;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Beyond Compare 4 launcher.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    [Traits(Kind.Diff, Rating.PAID_DIFF)]
    public class BeyondCompare4Reporter : ReporterBase
    {
        static BeyondCompare4Reporter()
        {
            switch (Environment.OSVersion.Platform)
            {
                default:
                case PlatformID.Win32NT:
                    _exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Beyond Compare 4", "BCompare.exe");
                    break;

                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    _exePath = null;// TODO: Figure out where beyond comparer is installed on mac and Linux.
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyondCompare4Reporter"/> class.
        /// </summary>
        public BeyondCompare4Reporter() : base(_exePath, "\"{0}\" \"{1}\"", true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyondCompare4Reporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> the test will be paused until the application is closed.</param>
        public BeyondCompare4Reporter(bool shouldPause) : base(_exePath, "\"{0}\" \"{1}\"", shouldPause)
        { }

        private static string _exePath;
    }
}