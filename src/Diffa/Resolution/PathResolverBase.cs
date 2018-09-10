using System.Reflection;

namespace Acklann.Diffa.Resolution
{
    public abstract class PathResolverBase : IPathResolver
    {
        public static string GetProjectDirectory()
        {
            string dir = Assembly.GetCallingAssembly().Location;
            

            return dir;
        }

        public abstract string GetAcutalResultPath(object data = null);


        public abstract string GetExpectedResultPath(object data = null);
    }
}