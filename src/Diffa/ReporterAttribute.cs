using System;

namespace Acklann.Diffa
{
    [AttributeUsage((AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method), AllowMultiple = false, Inherited = true)]
    public sealed class ReporterAttribute : Attribute
    {
        public ReporterAttribute(Type reporterType)
        {
        }
    }
}