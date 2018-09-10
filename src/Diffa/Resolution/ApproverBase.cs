using System;

namespace Acklann.Diffa.Resolution
{
    public abstract class ApproverBase<T> : IApprover
    {
        public ApproverBase(IPathResolver resovler)
        {
            Resolver = resovler ?? throw new ArgumentNullException(nameof(resovler));
        }

        protected readonly IPathResolver Resolver;

        public abstract bool Approve(T subject);

        public abstract void Report(T subject);

        #region IApprover

        bool IApprover.Approve(object subject) => Approve((T)subject);

        #endregion IApprover
    }
}