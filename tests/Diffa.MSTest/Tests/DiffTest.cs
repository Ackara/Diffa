using Acklann.Diffa.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    [Use(typeof(BeyondCompare4Reporter))]
    [SaveFilesAtAttribute(ApprovedResults)]
    public class DiffTest
    {
        private const string ApprovedResults = "ApprovedResults";

        [TestMethod]
        public void Can_approve_an_object()
        {
            Diff.Approve(12345);
        }

        [TestMethod]
        public void Can_specify_the_extension_when_approving_text()
        {
            Diff.Approve("{ \"name\": \"John Doe\", \"age\": \"84\" }", "json");
        }

        [TestMethod]
        public void Can_approve_a_file()
        {
            var sampleFile = Path.GetTempFileName();
            File.WriteAllText(sampleFile, "I want this file approved.");

            Diff.ApproveFile(sampleFile);
        }

        [TestMethod]
        public async Task Can_be_used_with_async_tests()
        {
            await Task.Delay(250);
            Diff.ApproveFromAsync("I can handle async methods.", ".txt", ApprovedResults);
        }

        [DataTestMethod]
        [DataRow('a'), DataRow('b')]
        public void Can_be_used_with_data_driven_tests(char arg)
        {
            Diff.Approve($"This is a data-driven test ({arg})", ".txt", arg);
        }

        [TestMethod]
        public void Can_be_used_when_not_directly_invoked_from_a_test_method()
        {
            IndirectTestCollection.RunIndirectTestA();
        }

        [TestMethod]
        [Use(typeof(VisualStudioReporter))]
        public void Should_launch_reporter_attached_test_method()
        {
            Diff.Approve("If you see this it was reported directory from the test method.");
        }
    }
}