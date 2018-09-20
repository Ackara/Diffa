using Acklann.Diffa.Resolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;
using System.Text;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    public class BinaryApproverTest
    {
        [TestMethod]
        public void Returns_true_when_two_txt_files_are_the_same()
        {
            // Arrange
            var sut = new BinaryApprover();
            var content = Encoding.UTF8.GetBytes("aye yah oou");

            var approvedFile = Path.GetTempFileName();
            File.WriteAllBytes(approvedFile, content);
            var resultFile = Path.Combine(Path.GetTempPath(), $"{nameof(BinaryApproverTest)}-areEqual.txt");

            // Act
            var approved = sut.Approve(content, resultFile, approvedFile, out string reason);

            foreach (var file in new[] { resultFile, approvedFile })
            {
                if (File.Exists(file)) File.Delete(file);
            }

            // Assert
            approved.ShouldBeTrue();
        }

        [TestMethod]
        public void Returns_false_when_two_txt_files_are_not_the_same()
        {
            // Arrange
            var sut = new BinaryApprover();

            var content = Encoding.UTF8.GetBytes("yah yah yah.");
            var resultFile = Path.Combine(Path.GetTempPath(), $"{nameof(BinaryApproverTest)}.result.txt");
            var approvedFile = Path.Combine(Path.GetTempPath(), $"{nameof(BinaryApproverTest)}.approved.txt");

            // Act
            foreach (var file in new[] { resultFile, approvedFile })
            {
                if (File.Exists(file)) File.Delete(file);
            }

            var approved = sut.Approve(content, resultFile, approvedFile, out string reason);
            var resultFileExist = File.ReadAllBytes(resultFile).Length >= content.Length;
            var approvedFileExist = File.Exists(approvedFile);

            // Assert
            approved.ShouldBeFalse();
            resultFileExist.ShouldBeTrue();
            approvedFileExist.ShouldBeTrue();
        }
    }
}