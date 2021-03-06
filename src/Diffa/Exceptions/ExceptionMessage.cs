﻿using System.IO;

namespace Acklann.Diffa.Exceptions
{
    internal static class ExceptionMessage
    {
        //TODO: Should localize exception messages.

        public static readonly string IssuesLink = "https://github.com/Ackara/Diffa/issues";

        public static string GetTestFrameworkNotSupported()
        {
            string supportedFrameworks = string.Join(", ", Resolution.KnownTestFramework.Names);
            return $@"The test framework you're using is not supported; Only the following test frameworks are currently supported: [{supportedFrameworks}].";
        }

        public static string GetResultWasNotApproved(string resultFilePath, string approvedFilePath, string reasonWhyFileWasRejected)
        {
            string more_info = string.IsNullOrEmpty(reasonWhyFileWasRejected) ? string.Empty : $"Additional Information:\r\n{reasonWhyFileWasRejected}\r\n";

            return $@"
{Path.GetFileName(resultFilePath)} contents is not the same as {Path.GetFileName(approvedFilePath)}
{more_info}
Solution:
{copyCommand()}
".TrimEnd();

            string copyCommand()
            {
                switch (System.Environment.OSVersion.Platform)
                {
                    default:
                    case System.PlatformID.Win32NT:
                        return $"cmd /c move /Y \"{resultFilePath}\" \"{approvedFilePath}\"";

                    case System.PlatformID.Unix:
                        return $"cp '{resultFilePath}' '{approvedFilePath}'";
                }
            }
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