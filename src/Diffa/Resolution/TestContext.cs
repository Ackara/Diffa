using Acklann.Diffa.Reporters;
using System.IO;

namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Represents a the state of a test.
    /// </summary>
    public readonly struct TestContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestContext"/> struct.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="subDirectory">The sub directory.</param>
        /// <param name="reporter">The reporter.</param>
        public TestContext(string methodName, string className, string sourceFile, string subDirectory, ReporterAttribute reporter)
        {
            if (string.IsNullOrEmpty(sourceFile)) throw new System.ArgumentNullException(nameof(sourceFile), $"Could not resolve {className} source file.");

            ReporterAttribute = reporter;
            TestClassName = className;
            TestMethodName = methodName;
            SubDirectory = subDirectory ?? string.Empty;
            SourceDirectory = Path.GetDirectoryName(sourceFile);
        }

        /// <summary>
        /// A null instance.
        /// </summary>
        public static readonly TestContext Empty = new TestContext();

        /// <summary>
        /// The reporter attribute.
        /// </summary>
        public readonly ReporterAttribute ReporterAttribute;

        /// <summary>
        /// The test class name.
        /// </summary>
        public readonly string TestClassName;

        /// <summary>
        /// The test method name.
        /// </summary>
        public readonly string TestMethodName;

        /// <summary>
        /// The approved files sub directory.
        /// </summary>
        public readonly string SubDirectory;

        /// <summary>
        /// The source file directory.
        /// </summary>
        public readonly string SourceDirectory;

        /// <summary>
        /// The test-project full path.
        /// </summary>
        public static string ProjectDirectory { get; set; }

        internal bool IsNotInstaniated => string.IsNullOrEmpty(SourceDirectory);
    }
}