using Acklann.Diffa.Resolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    public class ContextFileResolverTestB
    {
        [TestMethod]
        public void Should_return_path_honoring_the_attribute_on_the_assembly()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(Sample.ProjectDirectory, nameof(Tests), nameof(Diffa), $"{nameof(ContextFileResolverTestB)}-{nameof(Should_return_path_honoring_the_attribute_on_the_assembly)}.approved.txt");

            // Act
            var result1 = sut.GetApprovedFilePath(".txt");
            var result3 = StaticFunc(sut);
            var result2 = localFunc();

            // Assert
            result1.ShouldBe(expectedPath);
            result2.ShouldBe(expectedPath);
            result2.ShouldBe(expectedPath);

            string localFunc() => sut.GetApprovedFilePath(".txt");
        }

        private static string StaticFunc(ContextualFileResolver sut) => sut.GetApprovedFilePath(".txt");
    }
}