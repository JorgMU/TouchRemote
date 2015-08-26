using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TouchRemote
{
  public enum OptionActions { Nothing, Update, Exit }

  public partial class Form1 : Form
  {
    private static readonly string _exePath = Environment.GetCommandLineArgs()[0];
    private static readonly string _exeName = Path.GetFileName(_exePath);
    private static readonly string _confPath = Path.Combine
      (Path.GetDirectoryName(_exePath), 
      Path.GetFileNameWithoutExtension(_exeName) + ".conf"
    );

    private static Dictionary<string, TouchButton> _buttons;
    private static List<string> _groups;
    private static string _active;

    private bool _adjustingForm = false;
    private int _lastButtonHeight = 0;
    private int _lastButtonCount = 0;

    private const int SpacerY = -1;

    private const int MINBUTTONWIDTH = 48;
    private const int MINBUTTONHEIGHT = 32;
    private const int DEFAULTBUTTONWIDTH = 64;
    private const int DEFAULTBUTTONHEIGHT = 48;

    const int WM_NCRBUTTONDOWN = 0xA4;
    const int WS_EX_NOACTIVATE = 0x08000000;

    public Form1()
    {
      InitializeComponent();      
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      focusLabel.SendToBack();

      _buttons = new Dictionary<string, TouchButton>();

      FileInfo fi = new FileInfo(_confPath);
      if (fi.Exists) LoadButtons(fi);
      else
      {
        LoadDefaults();
        DialogResult dr = MessageBox.Show("Would you like to create a default conf file?", "TouchRemot", MessageBoxButtons.YesNo);
        if (dr == DialogResult.Yes) SaveButtons(fi);
      }

      UpdateGroups();
      ResetButtons();
      LoadLocation();
    }

    public void ActivateGroup(string GroupName)
    {
      if (_groups.Contains(GroupName))
        _active = GroupName;
      else if (!_groups.Contains(_active))
        if (_groups.Count > 0) _active = _groups[0];
        else _active = "";

      if (_active == "") this.Text = "TRemote";
      else this.Text = _active;
    }

    private void UpdateGroups()
    {
      _groups = new List<string>();
      foreach (TouchButton tb in _buttons.Values)
        if (!_groups.Contains(tb.Group))
          _groups.Add(tb.Group);
    }

    protected void ResetButtons()
    {
      List<string> toRemove = new List<string>();

      foreach (Control o in this.Controls)
        if (o.GetType() == typeof(Button))
          toRemove.Add(o.Name);

      foreach (string s in toRemove)
      {
        Control c = this.Controls[s];
        this.Controls.Remove(c);
        c.Dispose();
      }

      foreach (string bn in _buttons.Keys)
        if(_buttons[bn].Group == _active)
          AddButtonToForm(bn);

      AdjustForm();
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_NCRBUTTONDOWN)
        titleBar_rightButtonDown();
      else     
        base.WndProc(ref m);
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams ret = base.CreateParams;
        ret.ExStyle |= WS_EX_NOACTIVATE;
        return ret;
      }
    }

    private void shared_GotFocus(object sender, EventArgs e)
    {
      focusLabel.Focus();
    }

    private void titleBar_rightButtonDown()
    {
      this.TopMost = false;
      OptionsForm of = new OptionsForm(_groups, _active);
      of.Location = this.Location;
      of.Refresh();
      of.ShowDialog(this);
      OptionActions oa = of.Action;
      this.TopMost = true;

      if (oa == OptionActions.Exit) this.Close();
      else if (oa == OptionActions.Update)
      {
        ActivateGroup(of.ActiveGroup);
        ResetButtons();
      }
    }

    private void managedButton_MouseUp(object sender, MouseEventArgs e)
    {
      if (sender.GetType() != typeof(Button)) return;
      Button b = (Button)sender;

      if (e.Button == MouseButtons.Left)
        DoButtonAction(b.Name);

      focusLabel.Focus();
    }

    private void DoButtonAction(string ButtonName)
    {
      TouchButton b = _buttons[ButtonName];
      SendKeys.Send(b.Keys);
    }

    private void AddButtonToForm(string Name)
    {
      Button b = new Button();
      b.Padding = new Padding(0);
      b.Text = Name;
      b.Name = Name;
      b.Location = new System.Drawing.Point(0, 0);
      b.Size = new System.Drawing.Size(MINBUTTONWIDTH, MINBUTTONHEIGHT);
      b.UseVisualStyleBackColor = false;
      b.MouseUp += new MouseEventHandler(managedButton_MouseUp);
      b.GotFocus += new EventHandler(shared_GotFocus);
      this.Controls.Add(b);
    }

    private void AdjustForm()
    {
      if (_adjustingForm) return;
      if (_buttons == null || _buttons.Count < 1) return;
      if (_groups == null || _groups.Count < 1) return;
      if (_active == null || _active == "") return;

      int bc = 0;
      foreach (Control c in this.Controls)
        if (c.GetType() == typeof(Button)) bc++;

      if (bc < 1) return;
      _adjustingForm = true; //prevent us from stepping on our own toes

      //difference between form and client area
      int adjW = this.Width - this.ClientRectangle.Width;
      int adjH = this.Height - this.ClientRectangle.Height;

      //since we want perfect fit, we will always reset the size based on the number of buttons
      int ch = this.ClientRectangle.Height / bc + (bc * SpacerY); //first guess

      //if the last button count was 0, both 'last' values are not useful
      if (_lastButtonCount == 0)
      {
        ch = DEFAULTBUTTONHEIGHT;
        _lastButtonHeight = ch;
        _lastButtonCount = bc;
        this.Width = DEFAULTBUTTONWIDTH + adjW;
      }

      //if the number of buttons has changed, try to reuse the button height
      if(_lastButtonCount != bc) ch = _lastButtonHeight;

      if (ch < MINBUTTONHEIGHT) ch = MINBUTTONHEIGHT;

      //force exact height base new calculated height (ch)
      this.Height = bc * (ch + SpacerY) + adjH;

      int cw = this.ClientRectangle.Width;
      if (cw < MINBUTTONWIDTH)
      {
        cw = MINBUTTONWIDTH;
        this.Width = cw + adjW;
      }

      //save to try and reuse if the button count changes
      _lastButtonHeight = ch;
      _lastButtonCount = bc;

      //Now adjust the buttons to fit the form
      int i = 0;
      foreach (Control o in this.Controls)
        if(o.GetType().ToString() == "System.Windows.Forms.Button")
        {
          int newY = i * (ch + SpacerY);
          o.Location = new Point(0, newY);
          o.Size = new Size(cw, ch);
          o.TabIndex = i;
          i++;
        }

      focusLabel.Left = this.Width + 10;
      focusLabel.Top = this.Height + 10;

      _adjustingForm = false;
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
      if (_adjustingForm) return; //don't react to my own actions
      if (_buttons == null) return; //don't do anything if settings have not been loaded
      AdjustForm();
    }

    public void SaveButtons(FileInfo OptionsFile)
    {
      StreamWriter sw = null;
      try
      {
        sw = new StreamWriter(OptionsFile.OpenWrite());
        sw.WriteLine(@"#SendKeys info: https://msdn.microsoft.com/en-US/library/system.windows.forms.sendkeys.send(v=vs.110).aspx");
        sw.WriteLine(@"#Example Special Keys: {UP} {DOWN} {LEFT} {Right} {DEL} {BS} {TAB}");
        sw.WriteLine("active=" + _active);
        foreach (TouchButton b in _buttons.Values)
          sw.WriteLine("button={0}|{1}|{2}", b.Group, b.Name, b.Keys);
      }
      catch (SystemException se) { ShowError(se); }
      finally { if (sw != null) sw.Close(); }
    }

    public void LoadButtons(FileInfo OptionsFile)
    {
      if (!OptionsFile.Exists) return;

      string newActive = "";

      StreamReader sr = null;
      try
      {
        sr = OptionsFile.OpenText();
        while (!sr.EndOfStream)
        {
          string line = sr.ReadLine();
          if (line.StartsWith("#")) continue;
          string[] parts = line.Split("=".ToCharArray(), 2);
          if (parts.Length < 2) continue;
          switch (parts[0])
          {
            case "button":
              TouchButton tb = new TouchButton(parts[1]);
              if (tb.Name != "") _buttons.Add(tb.Name, tb);
              break;
            case "active":
              newActive = parts[1];
              break;
            default:
              break;
          }
        }
      }
      catch (SystemException se) { ShowError(se); }
      finally { if (sr != null) sr.Close(); }

      UpdateGroups();
      ActivateGroup(newActive);
    }

    public void LoadDefaults()
    {
      _buttons.Add("Cut", new TouchButton("CCP|Cut|^x"));
      _buttons.Add("Copy", new TouchButton("CCP|Copy|^c"));
      _buttons.Add("Paste", new TouchButton("CCP|Paste|^v"));
      _buttons.Add("Home", new TouchButton("CCP|Home|{Home}"));
      _buttons.Add("End", new TouchButton("CCP|End|{End}"));
      _buttons.Add("SA", new TouchButton("CCP|SA|^a"));
      UpdateGroups();
      ActivateGroup("CCP");
    }

    private void ShowError(SystemException se)
    {
      string msg = se.Message;
      if (se.InnerException != null) msg += "\r\n" + se.InnerException.Message;
      MessageBox.Show(msg, "TouchRemoteOptions", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }


    private const string REGROOT = @"Software\Jorg\TouchRemote";
    private void SaveLocation()
    {
      RegistryKey rk = Registry.CurrentUser.OpenSubKey(REGROOT, true);
      if (rk == null) rk = Registry.CurrentUser.CreateSubKey(REGROOT, RegistryKeyPermissionCheck.ReadWriteSubTree);
      if (rk == null) return;
      rk.SetValue("Left", this.Left, RegistryValueKind.DWord);
      rk.SetValue("Top", this.Top, RegistryValueKind.DWord);
      rk.SetValue("Width", this.Width, RegistryValueKind.DWord);
      rk.SetValue("Height", this.Height, RegistryValueKind.DWord);
    }

    private void LoadLocation()
    {
      RegistryKey rk = Registry.CurrentUser.OpenSubKey(REGROOT, RegistryKeyPermissionCheck.ReadSubTree);
      if (rk == null) return;

      int newLeft = (int)rk.GetValue("Left", 0);
      int newTop = (int)rk.GetValue("Top", 0);
      int newWidth = (int)rk.GetValue("Width", 0);
      int newHeight = (int)rk.GetValue("Height", 0);

      if (newLeft != 0) this.Left = newLeft;
      if (newTop != 0) this.Top = newTop;
      if (newWidth != 0) this.Width = newWidth;
      if (newHeight != 0) this.Height = newHeight;

      this.Refresh();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      SaveLocation();
    }

    public class TouchButton
    {
      public string Group;
      public string Name;
      public string Keys;

      public TouchButton()
      {
        this.Group = "";
        this.Name = "";
        this.Keys = "";
      }

      public TouchButton(string Group, string Name, string Keys)
      {
        this.Group = Group;
        this.Name = Name;
        this.Keys = Keys;
      }

      public TouchButton(string EncodedButton) : this()
      {
        string[] subparts = EncodedButton.Split("|".ToCharArray(), 3);
        if (subparts.Length < 3) return;
        Group = subparts[0].Trim();
        Name = subparts[1].Trim();
        Keys = subparts[2];
      }
    }

    
  }
}
  