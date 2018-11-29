using System;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Indicates that a test method, class or the entire assembly/project should use the specified <see cref="IReporter"/>.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage((AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method), AllowMultiple = false, Inherited = false)]
    public sealed class ReporterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReporterAttribute"/> class.
        /// </summary>
        /// <param name="reporterType">Type of the reporter.</param>
        public ReporterAttribute(Type reporterType) : this(reporterType, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReporterAttribute"/> class.
        /// </summary>
        /// <param name="reporterType">Type of the reporter.</param>
        /// <param name="interrupt">if set to <c>true</c> the test will pause until the reporter is closed.</param>
        public ReporterAttribute(Type reporterType, bool interrupt)
        {
            Reporter = reporterType;
            ShouldInterrupt = interrupt;
        }

        /// <summary>
        /// The <see cref="Reporters.IReporter"/> type.
        /// </summary>
        public readonly Type Reporter;

        /// <summary>
        /// Determines whether test execution should be pause when a reporter is launched.
        /// </summary>
        public readonly bool ShouldInterrupt;
    }
}