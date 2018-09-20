using System;
using System.IO;
using System.Linq;

namespace Acklann.Diffa
{
	public static partial class SampleFile
	{
		public const string FOLDER_NAME = "Samples";

		public static readonly string ProjectDirectory = "";

		public static string DirectoryName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_NAME);
        
		public static FileInfo GetFile(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            string searchPattern = $"*{Path.GetExtension(fileName)}";

            string appDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_NAME);
            return new DirectoryInfo(appDirectory).EnumerateFiles(searchPattern, SearchOption.AllDirectories)
                .First(x => x.Name.Equals(fileName, StringComparison.CurrentCultureIgnoreCase));
        }

		public static FileInfo GetGiffile() => GetFile(@"gifFile.gif");

		public static FileInfo GetInvalid_Xmlfile() => GetFile(@"invalid_xmlFile.xml");

		public static FileInfo GetTextfile() => GetFile(@"textFile.txt");

		public static FileInfo GetXmlfile() => GetFile(@"xmlFile.xml");

		public static FileInfo GetXmlschema() => GetFile(@"xmlSchema.xsd");

	}
}
