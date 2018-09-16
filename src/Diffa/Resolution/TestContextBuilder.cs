namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Represents a the state of a test.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Resolution.IContextBuilder" />
    public sealed class TestContextBuilder : IContextBuilder
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public TestContext Context => CreateContext();

        /// <summary>
        /// Gets or sets the name of the test class.
        /// </summary>
        /// <value>
        /// The name of the test class.
        /// </value>
        public string TestClassName { get; set; }

        /// <summary>
        /// Gets or sets the name of the test method.
        /// </summary>
        /// <value>
        /// The name of the test method.
        /// </value>
        public string TestMethodName { get; set; }

        /// <summary>
        /// Gets or sets the sub directory.
        /// </summary>
        /// <value>
        /// The sub directory.
        /// </value>
        public string SubDirectory { get; set; }

        /// <summary>
        /// Gets or sets the source file.
        /// </summary>
        /// <value>
        /// The source file.
        /// </value>
        public string SourceFile { get; set; }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns></returns>
        public TestContext CreateContext()
        {
            return new TestContext(TestMethodName, TestClassName, SourceFile, SubDirectory, null);
        }
    }
}