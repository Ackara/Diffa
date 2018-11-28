using System;

namespace Acklann.Diffa
{
    /// <summary>
    /// Specifies the sub-folder in which an approved-file should be saved, when applied to a test-method, test-class or the assembly.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage((AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method), AllowMultiple = false, Inherited = true)]
    public sealed class ApprovedFolderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovedFolderAttribute"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ApprovedFolderAttribute(string path)
        {
            Path = path;
        }

        /// <summary>
        /// The relative path.
        /// </summary>
        public readonly string Path;
    }
}