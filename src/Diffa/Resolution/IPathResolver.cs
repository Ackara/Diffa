namespace Acklann.Diffa.Resolution
{
    public interface IPathResolver
    {
        string GetAcutalResultPath(object data = default);

        string GetExpectedResultPath(object data = default);
    }
}