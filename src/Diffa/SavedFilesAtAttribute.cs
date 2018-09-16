using System;

namespace Acklann.Diffa
{
    /// <summary>
    /// Indicates that a test method, class or the assembly/project should save approved files in the specified folder.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage((AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method), AllowMultiple = false, Inherited = true)]
    public sealed class SaveFilesAtAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFilesAtAttribute"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public SaveFilesAtAttribute(string path)
        {
            Path = path;
        }

        /// <summary>
        /// The path
        /// </summary>
        public readonly string Path;
    }
}