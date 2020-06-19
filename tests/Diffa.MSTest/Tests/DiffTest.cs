using Acklann.Diffa.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Acklann.Diffa.Tests
{
    [TestClass]
    [ApprovedFolder(ApprovedResults)]
    public class DiffTest
    {
        private const string
            ApprovedResults = "ApprovedResults",
            Xmlns = "http://tempuri.org/sample.xsd";

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
            var sampleFile = Path.Combine(Path.GetTempPath(), $"diffa-{nameof(Can_approve_an_object)}.txt");
            File.WriteAllText(sampleFile, "I want this file approved.");

            Diff.ApproveFile(sampleFile);
        }

        [TestMethod]
        public async Task Can_be_used_with_async_tests()
        {
            await Task.Run(() => { System.Diagnostics.Debug.WriteLine("do work!"); });
            Diff.Approve("I can handle async methods.", default, ".txt", ApprovedResults);
        }

        [DataTestMethod]
        [DataRow('a'), DataRow('b')]
        public void Can_be_used_with_data_driven_tests(char arg)
        {
            Diff.Approve($"This is a data-driven test ({arg})", default, ".txt", arg);
        }

        [TestMethod]
        public void Can_be_used_when_not_directly_invoked_from_a_test_method()
        {
            IndirectTestCollection.RunIndirectTestA();
        }

        [TestMethod]
        [Reporter(typeof(VisualStudioReporter))]
        public void Should_launch_reporter_attached_test_method()
        {
            Diff.Approve("If you see this it was reported directory from the test method.");
        }

        [TestMethod]
        [Reporter(typeof(NullReporter))]
        public void Can_use_a_xsd_to_approve_a_xml_file()
        {
            var schemaFile = Sample.GetXmlschema().FullName;
            var sampleFile = Sample.GetXmlfile();
            var invaildFile = Sample.GetInvalid_Xmlfile();

            Diff.ApproveXml(sampleFile.OpenRead(), schemaFile, Xmlns);
            Diff.ApproveXml(File.ReadAllText(sampleFile.FullName), schemaFile, Xmlns);

            Should.Throw<Exceptions.ResultNotApprovedException>(() =>
            {
                Diff.ApproveXml(invaildFile.OpenRead(), schemaFile, Xmlns);
            });
        }

        [TestMethod]
        public void Can_serialize_object_to_json()
        {
            Diff.ApproveJson(new Fakes.Album()
            {
                Length = 64,
                Name = "What a time to be alive",
                RelaseDate = new DateTime(2015, 09, 05),
                Artists = new string[] { "Drake", "Future" }
            });
        }

        [TestMethod]
        public void Can_serialize_object_to_xml()
        {
            Diff.ApproveXml(new Fakes.Person()
            {
                Age = 28,
                Name = "Mary Jane"
            }, Sample.GetXmlschema().FullName, Xmlns);
        }

        [TestMethod]
        public void Can_approve_a_list()
        {
            var sample = new string[] { "dog", "cat", "rat" };
            Diff.ApproveAll(sample);
        }
    }
}