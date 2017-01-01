using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{
	public static class OneDrive
	{
		public static string Folder
		{
			get
			{
				string[] regKeys = new string[] {
					"HKEY_CURRENT_USER\\Software\\Microsoft\\SkyDrive",
					"HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\SkyDrive",
					"HKEY_CURRENT_USER\\Software\\Microsoft\\OneDrive"
				};
				foreach (var regKey in regKeys)
				{
					var folder = Registry.GetValue(regKey, "UserFolder", null);
					if (folder != null && Directory.Exists(folder.ToString())) return folder.ToString();
				}
				return null;
			}
		}

		public static string CombinePath(string fileName, bool createFolder = true)
		{
			string result = Path.Combine(Folder, fileName);
			if (createFolder)
			{
				string folder = Path.GetDirectoryName(result);
				if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
			}
			return result;
		}

		public static bool Exists()
		{
			return (!string.IsNullOrEmpty(Folder));
		}
	}
}
