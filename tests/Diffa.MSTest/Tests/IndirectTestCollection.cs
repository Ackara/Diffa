using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Acklann.Diffa.Tests
{
    internal class IndirectTestCollection
    {
        public static void RunIndirectTestA()
        {
            Diff.Approve($"{nameof(Diff)}.{nameof(Diff.Approve)} was not invoked directly from the test method.");
        }
    }
}