using Acklann.Diffa.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    public class ReporterLaunchTest
    {
        [DataTestMethod]
        [DataRow(typeof(NotepadPlusPlusReporter), true)]
        [DataRow(typeof(BeyondCompare4Reporter), true)]
        [DataRow(typeof(BeyondCompare4Reporter), true)]
        [DataRow(typeof(VisualStudioReporter), true)]
        [DataRow(typeof(NullReporter), true)]
        public void Can_launch_all_reporters_installed_on_this_machine(Type reporterType, bool verifiedThatItLaunched /* because launching all reporters all at once is annoying. */)
        {
            var fileA = Path.ChangeExtension(Path.GetTempFileName(), ".txt");
            File.WriteAllText(fileA, $"If you see this the \"{reporterType.Name}\" was launched successfully.");

            var fileB = SampleFile.GetTextfile().FullName;
            var reporter = (IReporter)Activator.CreateInstance(reporterType, args: false);

            if (reporter.CanLaunch())
            {
                if (verifiedThatItLaunched == false) reporter.Launch(fileA, fileB);
            }
            else
            {
                Assert.Inconclusive($"{reporterType.Name} might not be installed on this machine. Location:'{reporter}'");
            }
        }
    }
}