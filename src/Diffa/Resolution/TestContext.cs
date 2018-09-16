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
        public TestContext(string methodName, string className, string sourceFile, string subDirectory, UseAttribute reporter)
        {
            ReporterAttribute = reporter;
            TestClassName = className;
            TestMethodName = methodName;
            SubDirectory = subDirectory ?? string.Empty;
            SourceDirectory = Path.GetDirectoryName(sourceFile ?? string.Empty);
        }

        /// <summary>
        /// The reporter attribute.
        /// </summary>
        public readonly UseAttribute ReporterAttribute;

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

        internal bool IsNotInstaniated => string.IsNullOrEmpty(SourceDirectory);
    }
}