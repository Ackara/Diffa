using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Acklann.Diffa
{
    [TestClass]
    public class Startup
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            Resolution.TestContext.ProjectDirectory = Sample.ProjectDirectory;
        }
    }
}