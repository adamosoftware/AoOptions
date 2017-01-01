using AdamOneilSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionsTest
{
	public class Options : UserOptionsBase
	{
		public Options()
		{
			RecentFiles = new RecentFileList() { MaxCount = 4 };			
		}

		public FormPosition MainFormPosition { get; set; }
		public RecentFileList RecentFiles { get; set; }
	}
}
