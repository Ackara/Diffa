using System;
using System.IO;

namespace Acklann.Diffa.Resolution
{
    public class BinaryApprover : ApproverBase<Stream>
    {
        public BinaryApprover() : this(new StackTracePathResolver())
        {
        }

        public BinaryApprover(IPathResolver resolver) : base(resolver)
        {
        }

        public override bool Approve(Stream subject)
        {
            string e, a;

            

            throw new NotImplementedException();
        }

        public override void Report(Stream subject)
        {
            throw new NotImplementedException();
        }

        protected bool Compare()
        {
            Stream a, b;

            throw new System.NotImplementedException();
        }
    }
}