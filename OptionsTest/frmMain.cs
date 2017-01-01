using AdamOneilSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptionsTest
{
	public partial class frmMain : Form
	{
		private Options _options = null;

		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			_options = UserOptionsBase.Load<Options>("Options.xml", this);
			_options.RestoreFormPosition(_options.MainFormPosition, this);
			_options.TrackFormPosition(this, fp => { _options.MainFormPosition = fp; });

			_options.RecentFiles.MenuItem = recentFilesToolStripMenuItem;
			_options.RecentFiles.FileSelected += RecentFiles_FileSelected;			
		}

		private void RecentFiles_FileSelected(object sender, EventArgs e)
		{
			MessageBox.Show(_options.RecentFiles.SelectedFilename);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{

		}

		private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "All Files|*.*";
			if (dlg.ShowDialog() == DialogResult.OK) _options.RecentFiles.AddFile(dlg.FileName);
		}
	}
}
