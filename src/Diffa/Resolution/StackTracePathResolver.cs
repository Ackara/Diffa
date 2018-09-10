using System;
using System.Diagnostics;

namespace Acklann.Diffa.Resolution
{
    public class StackTracePathResolver : PathResolverBase
    {
        public StackTracePathResolver()
        {
            StackTrace st;
        }

        public override string GetAcutalResultPath(object data = default)
        {
            throw new NotImplementedException();
        }

        public override string GetExpectedResultPath(object data = default)
        {
            throw new NotImplementedException();
        }
    }
}