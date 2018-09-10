using System;

namespace Acklann.Diffa
{
    [AttributeUsage((AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method), AllowMultiple = false, Inherited = true)]
    public sealed class ApprovalFolderAttribute : Attribute
    {
        public ApprovalFolderAttribute(string path)
        {
            Location = path;
        }

        public readonly string Location;
    }
}