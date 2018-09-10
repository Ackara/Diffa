using Acklann.Diffa.Resolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    public class StackTracePathResolverTest
    {
        [TestMethod]
        public void Should_return_this_projects_directory()
        {
            // Arrange
            var sut = new StackTracePathResolver();

            var cwd = AppDomain.CurrentDomain.BaseDirectory;
            System.Diagnostics.Debug.WriteLine(cwd);

            // Act
            var result = PathResolverBase.GetProjectDirectory();

            // Assert
            result.ShouldBe(SampleFile.ProjectDirectory);
        }

        //[TestMethod]
        public void GetExpectedResultPath()
        {
            // Arrange
            var sut = new StackTracePathResolver();

            // Act
            var expectedPath = sut.GetExpectedResultPath();

            // Assert
            expectedPath.ShouldBe("");
        }
    }
}