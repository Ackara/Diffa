using System;

namespace Acklann.Diffa.Resolution
{
    internal class KnownTestFramework
    {
        public static readonly string[] TestMethodAttributeNames =
        {
            "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute, Microsoft.VisualStudio.TestPlatform.TestFramework",
            "Xunit.FactAttribute, xunit.core"
        };

        public static readonly string[] Names = Enum.GetNames(typeof(Kind));

        public enum Kind
        {
            MSTest,
            XUnit
        }
    }
}