using System;
using System.IO;

namespace Mathematica.Helpers
{
	public class FileHelper
	{
		private static string DocumentLibraryPath =
			Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

		public static string GetProgramDirectory()
		{
			string path = Path.Combine(DocumentLibraryPath, "NotMath");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			return path;
		}

		public static string FileExtension = "notmath";
	}
}