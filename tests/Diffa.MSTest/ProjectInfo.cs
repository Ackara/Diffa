using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace Acklann.Diffa
{
    public partial class SampleFile
    {
        static SampleFile()
        {
            Assembly assembly = typeof(SampleFile).Assembly;
            foreach (string name in assembly.GetManifestResourceNames())
                if (name.Contains("diffa"))
                {
                    using (var reader = new StreamReader(assembly.GetManifestResourceStream(name)))
                    {
                        var obj = JObject.Parse(reader.ReadToEnd());
                        ProjectDirectory = (obj["directory"].Value<string>()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                    }

                    break;
                }
        }
    }
}