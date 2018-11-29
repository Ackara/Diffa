using Acklann.Diffa.Resolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    [ApprovedFolder("ClassLvl")]
    public class ContextFileResolverTestA
    {
        private const string Id = "f9d0acb8-7f51-45e7-a56d-bcd768d69363";

        [TestMethod]
        [ApprovedFolder("MethodLvl")]
        public void Should_return_path_honoring_the_attribute_on_the_method()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(Sample.ProjectDirectory, nameof(Tests), "MethodLvl", $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_honoring_the_attribute_on_the_method)}[a,b].approved.txt");

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
            var expectedPath = Path.Combine(Sample.ProjectDirectory, nameof(Tests), "ClassLvl", $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_honoring_the_attribute_on_the_class)}[c,d].approved.txt");

            // Act
            var result1 = sut.GetApprovedFilePath(".txt", 'c', 'd');

            // Assert
            result1.ShouldBe(expectedPath);
        }

        [TestMethod, ApprovedName(Id)]
        public void Should_return_path_honoring_the_tuid()
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(Sample.ProjectDirectory, nameof(Tests), "ClassLvl", $"{Id}.approved.txt");

            // Act
            var result1 = sut.GetApprovedFilePath(".txt");

            // Assert
            result1.ShouldBe(expectedPath);
        }

        [DataTestMethod]
        [DataRow('a'), DataRow('b'), DataRow('c')]
        [ApprovedFolder(nameof(DataTestMethodAttribute))]
        public void Should_return_path_honoring_the_parameters_on_the_method(char arg)
        {
            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(Sample.ProjectDirectory, nameof(Tests), nameof(DataTestMethodAttribute), $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_honoring_the_parameters_on_the_method)}[{arg}].approved.txt");

            // Act
            var result = sut.GetApprovedFilePath(".txt", arg);

            // Assert
            result.ShouldBe(expectedPath);
        }

        [TestMethod]
        [ApprovedFolder("Async")]
        public async Task Should_return_path_to_async_test()
        {
            System.Diagnostics.Debug.WriteLine($"thread: '{Thread.CurrentThread.Name}'({Thread.CurrentThread.ManagedThreadId})");

            // Arrange
            var sut = new ContextualFileResolver();
            var expectedPath = Path.Combine(Sample.ProjectDirectory, nameof(Tests), "Async", $"{nameof(ContextFileResolverTestA)}-{nameof(Should_return_path_to_async_test)}.approved.txt");

            // Act
            await Task.Delay(100);
            var result1 = sut.GetApprovedFilePath(".txt");

            // Assert
            result1.ShouldBe(expectedPath);
        }
    }
}