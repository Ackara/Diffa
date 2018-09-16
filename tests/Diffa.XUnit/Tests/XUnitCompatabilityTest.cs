using Xunit;

namespace Acklann.Diffa.Tests
{
    public class XUnitCompatabilityTest
    {
        [Fact(DisplayName = nameof(Can_be_used_with_xunit))]
        public void Can_be_used_with_xunit()
        {
            Diff.Approve("This message confirms XUnit is supported.");
        }
    }
}