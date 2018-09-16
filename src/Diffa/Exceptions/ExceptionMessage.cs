namespace Acklann.Diffa.Exceptions
{
    internal static class ExceptionMessage
    {
        public static string TestFrameworkNotSupported()
        {
            // TODO: Write statement on how one can resolve the issue.
            string supportedFrameworks = string.Join(", ", Resolution.KnownTestFramework.Names);
            return $@"The test framework you're using is not supported; Only the following test frameworks are currently supported: [{supportedFrameworks}].";
        }

        public static string ResultWasNotApproved(string resultFilePath, string approvedFilePath, string reasonWhyFileWasRejected)
        {
            // TODO: Finish writing (result was not approved) exception message.
            return $@"{resultFilePath} is not the same as {approvedFilePath}
ADDITIONAL INFORMATION:
{reasonWhyFileWasRejected}";
        }
    }
}