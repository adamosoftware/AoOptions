# AoOptions
AoOptions library is a replacement for the Settings feature that comes with Visual Studio, offering XML persistence for user settings in WinForms apps. The built-in Settings class tracks with the product version, causing users to lose their settings if the program version changes. This is terrible behavior in my opinion! AoOptions fixes this, and adds some capabilities such as tracking form sizes/positions, supports encrypted properties, binds checked menu items, and offers MRU list capability.

To get started, create a class to hold user settings that inherits from `UserOptionsBase`. For example:

    public class UserOptions : UserOptionsBase
    {
      public FormPosition MainFormPosition { get; set; }
      [Encrypt]
      public string UserName { get; set; }
      [Encrypt]
      public string Password { get; set; }
      
      public DateTime? SomeDate { get; set; }
      public int? SomeValue { get; se;t }
      public bool Setting1 { get; set; }
      public bool Setting2 { get; set; }
    }

Then, use your settings class in a Form like this. Use the `Load` method to load previously saved options. The default save location is under `Environment.SpecialFolder.LocalApplicationData`, but you can use Dropbox and OneDrive also by using the other `Load` method overload.

    private UserOptions _options = null;
    
    private void frmMain_Load(object sender, EventArgs e)
    {
      // load saved options, and automatically save when the form closes
      _options = UserOptionsBase.Load<Options>("Options.xml", this);
      
      // restore the main form's position and track subsequent movement on the MainFormPosition property
      _options.RestoreFormPosition(_options.MainFormPosition, this);
      _options.TrackFormPosition(this, fp => { _options.MainFormPosition = fp });
    }

Use encrypted properties just like any other. There's no special handling required, you simply use the `Encrypt` attribute on the property definition. Encrypted properties are encrypted with DPAPI `DataProtectionScope.CurrentUser` by default. You can use the `LocalMachine` scope if you wish by indicating that in the `[Encrypt]` attribute constructor like so:

    [Encrypt(DataProtectionScope.LocalMachine)]
    public string Password { get; set; }

Note that encrypted properties are encrypted on disk, but clear in memory.

##Binding Settings to Checked Menu Items
You can bind a `bool` settings property to the Checked state of ToolStripMenuItem. There are two steps. 1) In your form's Load event, set the initial Checked state of the menu item according to the current option setting. 2) Use the `BindCheckedMenuItem` method to track subsequent changes to the check state. Example:

    setting1ToolStripMenuItem.Checked = _options.Setting1;
    _options.BindCheckedMenuItem(setting1ToolStripMenuItem, (menuItem) => { _options.Setting1 = menuItem.Checked; });

##Using Recent File List
You can have a list of recently used file names managed automatically with the `RecentFileList` class. Here are the steps, assuming your recent files property is called `RecentFiles`:

1. In your UserOptions class, add property called `RecentFiles` of type `RecentFileList`.
2. In the constructor of your UserOptions class, assign it to a new `RecentFileList`. The default maxCount is 4, but you can use a different value in the constructor argument.
3. In your form's Load event, assign the `RecentFiles.MenuItem` property to the ToolStripMenuItem that will serve as the parent container of the generated menu items. Also, handle the `RecentFiles.FileSelected` event to respond to user selections of the recent file list.

Here's what it looks like all put together -- the `UserOptions` class part:

    public class UserOptions : UserOptionsBase
    {
        public UserOptions()
        {
            RecentFiles = new RecentFileList();
        }
        
        public RecentFileList RecentFiles { get; set; }
    }

Here's what the form Load event would need. I have omitted some code for clarity as well as assumed that the menu items are present in the form designer:

    private void frmMain_Load(object sender, EventArgs e)
    {
        _options.RecentFiles.MenuItem = recentFilesToolStripMenuItem;
        _options.RecentFiles.FileSelected += RecentFiles_Selected;
    }
    
    private void RecentFiles_Selected(object sender, EventArgs e)
    {
        MessageBox(_options.RecentFiles.SelectedFilename);
    }
