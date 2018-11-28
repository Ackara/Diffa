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
        public ReporterAttribute(Type reporterType) : this(reporterType, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReporterAttribute"/> class.
        /// </summary>
        /// <param name="reporterType">Type of the reporter.</param>
        /// <param name="doNotPauseIfTestFails">if set to <c>true</c> [do not pause if test fails].</param>
        public ReporterAttribute(Type reporterType, bool doNotPauseIfTestFails)
        {
            Reporter = reporterType;
            ShouldPause = doNotPauseIfTestFails == false;
        }

        /// <summary>
        /// The <see cref="Reporters.IReporter"/> type.
        /// </summary>
        public readonly Type Reporter;

        /// <summary>
        /// Determines whether test execution should be pause when a reporter is launched.
        /// </summary>
        public readonly bool ShouldPause;
    }
}