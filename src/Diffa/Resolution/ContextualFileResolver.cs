using System.IO;

namespace Acklann.Diffa.Resolution
{
    /// <summary>
    /// Defines methods to retrieve the full path of a result and approved file.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Resolution.IFileResolver" />
    public class ContextualFileResolver : IFileResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualFileResolver"/> class.
        /// </summary>
        public ContextualFileResolver() : this(new StackTraceParser())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualFileResolver"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public ContextualFileResolver(IContextBuilder builder)
        {
            _contextBuilder = builder ?? throw new System.ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the result file path.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="args">The parameterized test arguments.</param>
        /// <returns></returns>
        public string GetResultFilePath(string fileExtension, params object[] args)
        {
            TestContext context = _contextBuilder.Context;
            return Path.Combine(Path.GetTempPath(), nameof(Diffa), $"{context.TestClassName}-{context.TestMethodName}{ToSuffix(args)}.result{AppendDot(fileExtension)}");
        }

        /// <summary>
        /// Gets the approved file path.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="args">The parameterized test arguments.</param>
        /// <returns></returns>
        public string GetApprovedFilePath(string fileExtension, params object[] args)
        {
            TestContext context = _contextBuilder.CreateContext();
            return Path.Combine(context.SourceDirectory, context.SubDirectory, $"{context.TestClassName}-{context.TestMethodName}{ToSuffix(args)}.approved{AppendDot(fileExtension)}");
        }

        #region Private Members

        private readonly IContextBuilder _contextBuilder;

        private static string ToSuffix(params object[] args)
        {
            return (args.Length >= 1 ? string.Format("[{0}]", string.Join(",", args)) : string.Empty);
        }

        private static string AppendDot(string fileExtension)
        {
            return (fileExtension.StartsWith(".") ? fileExtension : $".{fileExtension}");
        }

        #endregion Private Members
    }
}