namespace Acklann.Diffa.Asserters
{
    internal interface IAsserter
    {
        void Assert(object expected, object actual);
    }
}