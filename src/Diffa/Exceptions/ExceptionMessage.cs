namespace Acklann.Diffa.Exceptions
{
    internal static class ExceptionMessage
    {
        // Should localize exception message.

        public static readonly string IssuesLink = "https://github.com/Ackara/Diffa/issues";

        public static string GetTestFrameworkNotSupported()
        {
            string supportedFrameworks = string.Join(", ", Resolution.KnownTestFramework.Names);
            return $@"The test framework you're using is not supported; Only the following test frameworks are currently supported: [{supportedFrameworks}].";
        }

        public static string GetResultWasNotApproved(string resultFilePath, string approvedFilePath, string reasonWhyFileWasRejected)
        {
            return $@"{resultFilePath} is not the same as {approvedFilePath}

ADDITIONAL INFORMATION:
{reasonWhyFileWasRejected}";
        }

        public static string ReporterFailedToOpen(string reporterName, string executablePath, string args)
        {
            return $"Failed to open the {reporterName}; The following args were used filename:'{executablePath}' args:'{args}'";
        }

        public static string GetTestNotFoundMessage()
        {
            string supportedFrameworks = string.Join(", ", Resolution.KnownTestFramework.Names);
            return $@"Could not detect test framework.

Possible Reasons Why:
1) Optimizer inlined test methods.

Solution:
Disable code optimizer, you can do this by either
a) Add the elelment '<Optimize>false</Optimize>' to your test project file (.csproj or .vbproj).

b) In Visual Studio right-click the project go to `Propertes -> Build` then uncheck the 'Optimize' checkbox for the 'Release' configuration.

or

2)
{nameof(Diffa)} do not support you test framework. Currently only the following are supported [{supportedFrameworks}].
";
        }
    }
}