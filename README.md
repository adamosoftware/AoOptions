# AoOptions
AoOptions library is a replacement for the Settings feature that comes with Visual Studio, offering XML persistence for user settings in WinForms apps. The built-in Settings class tracks with the product version, causing users to lose their settings if the program version changes. This is terrible behavior in my opinion! AoOptions fixes this, and adds some capabilities such as tracking form sizes/positions and supporting encrypted properties.

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

