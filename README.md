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

Use encrypted properties just like any other. There's no special handling required, you simply use the `Encrypt` attribute on the property definition. Encrypted properties are encrypted with DPAPI `DataProtectionScope.CurrentUser`.

##Binding Settings to Checked Menu Items
There are two steps. 1) In your form's Load event, set the initial Checked state of the menu item according to the current option setting. 2) Use the `BindCheckedMenuItem` method to track subsequent changes to the check state. Example:

    setting1ToolStripMenuItem.Checked = _options.Setting1;
    _options.BindCheckedMenuItem(setting1ToolStripMenuItem, (menuItem) => { _options.Setting1 = menuItem.Checked; });

##Using Recent File List
