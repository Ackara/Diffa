using System;

namespace Acklann.Diffa
{
    /// <summary>
    /// Specifies the approved-file name, when applied to a test method.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ApprovedNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovedNameAttribute"/> class.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public ApprovedNameAttribute(string guid)
        {
            Guid = guid;
        }

        /// <summary>
        /// The unique identifier.
        /// </summary>
        public readonly string Guid;
    }
}