using System;

namespace Acklann.Diffa.Reporters
{
    internal static class Rating
    {
        public const sbyte PAID = 3;
        public const sbyte FREE = 2;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class RankAttribute : Attribute
    {
        public RankAttribute(int index)
        {
            Index = index;
        }

        public readonly int Index;
    }
}