﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamOneilSoftware
{
	public class RecentFileList : List<string>
	{
		private string _selectedFile = null;
		private ToolStripMenuItem _menuItem = null;
		private readonly int _maxCount;

		public RecentFileList(int maxCount = 4)
		{
			_maxCount = maxCount;
		}

		public event EventHandler FileSelected;

		public string SelectedFilename { get { return _selectedFile; } }

		public int MaxCount
		{
			get { return _maxCount; }
		}

		public ToolStripMenuItem MenuItem
		{
			get { return _menuItem; }
			set { _menuItem = value; BuildMenuItems(); }
		}

		public ToolStripSeparator Separator { get; set; }

		public void AddFile(string fileName)
		{
			if (Contains(fileName)) return;

			base.Insert(0, fileName);

			while (Count > MaxCount) base.RemoveAt(Count - 1);

			if (MenuItem != null)
			{
				// add as sub-menu items
				MenuItem.DropDownItems.Clear();
				BuildMenuItems();
			}
		}

		public void BuildMenuItems()
		{
			int index = 0;
			foreach (var item in this)
			{
				index++;
				FileMenuItem mnuFileItem = new FileMenuItem("&" + index.ToString() + " " + item);
				mnuFileItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
				mnuFileItem.Filename = item;
				mnuFileItem.Index = index;
				mnuFileItem.Click += mnuFileItem_Click;
				MenuItem.DropDownItems.Add(mnuFileItem);
			}
		}

		private void mnuFileItem_Click(object sender, EventArgs e)
		{
			FileMenuItem mi = sender as FileMenuItem;
			if (mi != null)
			{
				_selectedFile = mi.Filename;
				if (FileSelected != null) FileSelected(sender, e);
			}			
		}
	}

	public class FileMenuItem : ToolStripMenuItem
	{
		public FileMenuItem(string text) : base(text)
		{			
		}

		public int Index { get; set; }
		public string Filename { get; set; }
	}
}
