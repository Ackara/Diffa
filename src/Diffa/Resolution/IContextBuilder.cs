namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Defines methods to retrieve the state of a test.
    /// </summary>
    public interface IContextBuilder
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        TestContext Context { get; }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns></returns>
        TestContext CreateContext();
    }
}