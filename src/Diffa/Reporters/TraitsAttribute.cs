using System;

namespace Acklann.Diffa.Reporters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class TraitsAttribute : Attribute
    {
        public TraitsAttribute(Kind kind, int index)
        {
            Kind = kind;
            Index = index;
        }

        public readonly int Index;
        public readonly Kind Kind;
    }
}