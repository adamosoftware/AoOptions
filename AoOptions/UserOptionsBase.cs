using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AdamOneilSoftware
{
	public enum OptionsLocation
	{
		LocalAppData,
		Dropbox,
		OneDrive
	}

	public class UserOptionsBase
	{
		[Browsable(false)]
		[XmlIgnore]
		public string Filename { get; set; }						

		public static string GetFolder(OptionsLocation location)
		{
			var baseFolders = new Dictionary<OptionsLocation, string>()
			{
				{ OptionsLocation.LocalAppData, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) },
				{ OptionsLocation.Dropbox, Dropbox.PersonalFolder },
				{ OptionsLocation.OneDrive, OneDrive.Folder }
			};
			
			return Path.Combine(baseFolders[location], Application.CompanyName, Application.ProductName);							
		}

		public static T Load<T>(string fileName, Form form, OptionsLocation location = OptionsLocation.LocalAppData) where T : UserOptionsBase, new()
		{
			T result = Load<T>(fileName);

			form.FormClosing += delegate (object sender, FormClosingEventArgs e)
			{
				result.Save();
			};

			return result;
		}

		public static T Load<T>(string fileName, OptionsLocation location = OptionsLocation.LocalAppData) where T : UserOptionsBase, new()
		{
			fileName = Path.Combine(GetFolder(location), fileName);

			T result = new T();
			result.Filename = fileName;			
			if (File.Exists(fileName))
			{
				FileInfo fi = new FileInfo(fileName);
				if (fi.Length == 0) return new T() { Filename = fileName };
				XmlSerializer xs = new XmlSerializer(typeof(T));
				using (StreamReader reader = File.OpenText(fileName))
				{
					result = (T)xs.Deserialize(reader);
					result.Filename = fileName;
					reader.Close();
				}

				PropertyInfo[] allProps = typeof(T).GetProperties();
				foreach (var prop in allProps.Where(p => p.HasAttribute<EncryptAttribute>()))
				{
					DecryptProperty(result, prop);
				}
			}

			return result;
		}

		private static void DecryptProperty<T>(T result, PropertyInfo prop) where T : UserOptionsBase, new()
		{
			EncryptAttribute attr = prop.GetCustomAttribute<EncryptAttribute>();
			object currentValue = prop.GetValue(result);
			if (currentValue != null)
			{
				prop.SetValue(result, Encryption.Decrypt(currentValue.ToString(), attr.Scope));
			}
		}

		public void Save()
		{
			string path = Path.GetDirectoryName(Filename);
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			PropertyInfo[] encryptedProps = GetType().GetProperties().Where(p => p.HasAttribute<EncryptAttribute>()).ToArray();

			foreach (var prop in encryptedProps) EncryptProperty(prop);

			XmlSerializer xs = new XmlSerializer(this.GetType());
			using (StreamWriter writer = File.CreateText(Filename))
			{
				xs.Serialize(writer, this);
				writer.Close();				
			}

			foreach (var prop in encryptedProps) DecryptProperty(this, prop);			
		}

		private void EncryptProperty(PropertyInfo prop)
		{
			EncryptAttribute attr = prop.GetCustomAttribute<EncryptAttribute>();
			object currentValue = prop.GetValue(this);
			if (currentValue != null)
			{
				prop.SetValue(this, Encryption.Encrypt(currentValue.ToString(), attr.Scope));
			}
		}

		public void RestoreFormPosition(FormPosition formPosition, Form form)
		{
			if (formPosition == null) return;

			switch (formPosition.WindowState)
			{
				case FormWindowState.Maximized:
					form.WindowState = FormWindowState.Maximized;
					break;

				case FormWindowState.Normal:
					form.Location = formPosition.Location;
					form.Size = formPosition.Size;
					break;

				case FormWindowState.Minimized:
					// don't restore minimized -- user won't find it!
					break;
			} 
		}		

		public void TrackFormPosition(Form form, Action<FormPosition> callback)
		{
			form.LocationChanged += delegate(object sender, EventArgs e)
			{
				FormPosition fp = new FormPosition() { Location = form.Location, WindowState = form.WindowState, Size = form.Size };				
				callback.Invoke(fp);
			};

			form.SizeChanged += delegate(object sender, EventArgs e)
			{
				FormPosition fp = new FormPosition() { Location = form.Location, WindowState = form.WindowState, Size = form.Size };				
				callback.Invoke(fp);
			};
		}		

		public void BindCheckedMenuItem(ToolStripMenuItem menuItem, Action<ToolStripMenuItem> onClick)
		{
			menuItem.Click += delegate(object sender, EventArgs e)
			{
				menuItem.Checked = !menuItem.Checked;
				onClick.Invoke(menuItem);
			};
		}		
	}
}
