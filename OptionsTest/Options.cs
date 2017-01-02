using AdamOneilSoftware;

namespace OptionsTest
{
	public class Options : UserOptionsBase
	{
		public Options()
		{
			RecentFiles = new RecentFileList(4);			
		}

		public FormPosition MainFormPosition { get; set; }
		public RecentFileList RecentFiles { get; set; }

		[Encrypt]
		public string UserName { get; set; }
		[Encrypt]
		public string Passord { get; set; }
		
		public bool Setting1 { get; set; }
		public bool Setting2 { get; set; }
	}
}
