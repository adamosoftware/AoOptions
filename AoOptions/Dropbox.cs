using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{
	// thanks to https://www.dropbox.com/help/4584?path=desktop_client_and_web_app
	// requires reference to Newtonsoft.Json
	public static class Dropbox
	{
		public static string PersonalFolder
		{
			get { return GetFolder("personal"); }
		}		

		public static bool PersonalFolderExists()
		{
			return Directory.Exists(GetFolder("personal"));
		}

		// not tested
		public static string BusinessFolder
		{
			get { return GetFolder("business"); }
		}

		public static bool BusinessFolderExists()
		{
			return Directory.Exists(GetFolder("business"));
		}

		private static string GetFolder(string type)
		{
			const string infoJson = @"Dropbox\info.json";
			string infoFile = (new[] {
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), infoJson),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), infoJson)
			}.FirstOrDefault(s => File.Exists(s)));

			if (infoFile == null) return null;

			string content = File.ReadAllText(infoFile);
			var info = JObject.Parse(content);
            try
            {
                return info.SelectToken($"{type}.path").Value<string>();
            }
            catch
            {
                return null;
            }			
		}

		public static string BuildPersonalPath(string path, bool create = false)
		{
			string result = Path.Combine(PersonalFolder, path);
			string folder = Path.GetDirectoryName(result);
			if (create && !Directory.Exists(folder)) Directory.CreateDirectory(folder);
			return result;
		}
	}
}
