namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// The first known-available file editor on this machine.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    public class FileReporter : ReporterBase
    {
        static FileReporter()
        {
            var factory = new ReporterFactory();
            foreach (IReporter item in factory.GetReporters(false, Kind.Editor))
                if (item is ReporterBase reporter)
                {
                    _exePath = reporter._executablePath;
                    _args = reporter._format;
                    break;
                }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReporter"/> class.
        /// </summary>
        public FileReporter() : base(_exePath, _args, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReporter"/> class.
        /// </summary>
        /// <param name="shouldInterrupt">if set to <c>true</c> the test will be paused until the application is closed.</param>
        public FileReporter(bool shouldInterrupt) : base(_exePath, _args, shouldInterrupt)
        { }

        private static readonly string _exePath, _args;
    }
}