using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Acklann.Diffa
{
    [TestClass]
    public class Booter
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Resolution.TestContext.ProjectDirectory = Sample.ProjectDirectory;
        }
    }
}