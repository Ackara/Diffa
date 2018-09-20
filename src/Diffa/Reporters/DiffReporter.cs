namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// The first known-available diff tool on this machine.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    public class DiffReporter : ReporterBase
    {
        static DiffReporter()
        {
            var factory = new ReporterFactory();
            foreach (var item in factory.GetReporters(false, Kind.Diff))
                if (item is ReporterBase reporter)
                {
                    _exePath = reporter._executablePath;
                    _args = reporter._format;
                    break;
                }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffReporter"/> class.
        /// </summary>
        public DiffReporter() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffReporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> the test will be paused until the application is closed.</param>
        public DiffReporter(bool shouldPause) : base(_exePath, _args, shouldPause)
        {
        }

        private static readonly string _exePath, _args;
    }
}