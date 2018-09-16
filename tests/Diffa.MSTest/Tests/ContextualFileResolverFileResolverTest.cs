using Acklann.Diffa.Resolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;
using System.Threading.Tasks;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    [SaveFilesAtAttribute("ClassLvl")]
    public class ContextFileResolverTestA
    {
        [TestMethod]
        [SaveFilesAtAttribute("MethodLvl")]
        public void Should_return_path_honoring_the_attribute_on_the_method()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(SampleFile.ProjectDirectory, nameof(Tests), "MethodLvl", $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_honoring_the_attribute_on_the_method)}[a,b].approved.txt");

            // Act
            var result = sut.GetApprovedFilePath(".txt", 'a', 'b');

            // Assert
            result.ShouldBe(expectedPath);
        }

        [TestMethod]
        public void Should_return_path_honoring_the_attribute_on_the_class()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(SampleFile.ProjectDirectory, nameof(Tests), "ClassLvl", $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_honoring_the_attribute_on_the_class)}[c,d].approved.txt");

            // Act
            var result1 = sut.GetApprovedFilePath(".txt", 'c', 'd');

            // Assert
            result1.ShouldBe(expectedPath);
        }

        [DataTestMethod]
        [DataRow('a'), DataRow('b'), DataRow('c')]
        [SaveFilesAtAttribute(nameof(DataTestMethodAttribute))]
        public void Should_return_path_honoring_the_parameters_on_the_method(char arg)
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(SampleFile.ProjectDirectory, nameof(Tests), nameof(DataTestMethodAttribute), $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_honoring_the_parameters_on_the_method)}[{arg}].approved.txt");

            // Act
            var result = sut.GetApprovedFilePath(".txt", arg);

            // Assert
            result.ShouldBe(expectedPath);
        }

        //[TestMethod]
        [SaveFilesAtAttribute("Async")]
        /// TODO: Find a good soltuion for <see cref="ContextualFileResolver"/> to handle async methods.
        public async Task Should_return_path_to_async_test()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(SampleFile.ProjectDirectory, nameof(Tests), "Async", $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_to_async_test)}.approved.txt");

            // Act
            var result1 = sut.GetApprovedFilePath(".txt");
            await Task.Delay(100);

            // Assert
            result1.ShouldBe(expectedPath);
        }
    }

    [TestClass]
    public class ContextFileResolverTestB
    {
        [TestMethod]
        public void Should_return_path_honoring_the_attribute_on_the_assembly()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(SampleFile.ProjectDirectory, nameof(Tests), nameof(Diffa), $"{nameof(ContextFileResolverTestB)}-{nameof(Should_return_path_honoring_the_attribute_on_the_assembly)}.approved.txt");

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