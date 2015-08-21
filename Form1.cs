using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TouchRemote
{
  public enum OptionActions { Nothing, Update, Exit }

  public partial class Form1 : Form
  {
    private TouchMenuOptions _opt;

    private bool _adjustingForm = false;
    private int _lastButtonHeight = 0;
    private int _lastButtonCount = 0;

    private const int SpacerY = -1;

    private const int MINBUTTONWIDTH = 48;
    private const int MINBUTTONHEIGHT = 32;

    const int WM_NCRBUTTONDOWN = 0xA4;
    const int WM_NCRBUTTONUP = 0xA5;

    const int WS_THICKFRAME = 0x00040000;
    const int WS_CHILD = 0x40000000;
    const int WS_EX_NOACTIVATE = 0x08000000;
    const int WS_EX_TOOLWINDOW = 0x00000080;

    public Form1()
    {
      InitializeComponent();      
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      //focustLable is given the focus so that the buttons are never highlighted
      //this moves it out of the clipping area so it is not visible
      focusLabel.Location = new Point(this.ClientRectangle.Width + 32, 0);
      _opt = new TouchMenuOptions();
      ResetButtons();
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

      foreach (string bn in _opt.SelectedSet.Buttons.Keys)
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
      Button b = (Button)sender;
      focusLabel.Focus();
    }

    private void titleBar_rightButtonDown()
    {
      this.TopMost = false;
      OptionsForm of = new OptionsForm(ref _opt);
      of.ShowDialog();
      OptionActions oa = of.Action;
      this.TopMost = true;

      if (oa == OptionActions.Exit) this.Close();
      else if (oa == OptionActions.Update) ResetButtons();
    }

    private void managedButton_MouseUp(object sender, MouseEventArgs e)
    {
      if (sender.GetType() != typeof(Button)) return;
      Button b = (Button)sender;

      if (e.Button == MouseButtons.Right)
      {
      }
      else if (e.Button == MouseButtons.Left)
        DoButtonAction(b.Name);

    }

    private void DoButtonAction(string ButtonName)
    {
      TouchMenuOptions.TouchButton b = _opt.SelectedSet[ButtonName];

      switch (b.Type)
      {
        case ButtonType.Keystroke:
          SendKeys.Send(b.Keys);
          break;
        default:
          break;
      }
    }

    private void AddButtonToForm(string Name)
    {
      Button b = new Button();
      b.Padding = new Padding(0);
      b.Text = Name;
      b.Name = Name;
      b.Location = new System.Drawing.Point(0, 0);
      b.Size = new System.Drawing.Size(MINBUTTONWIDTH, MINBUTTONHEIGHT);
      b.UseVisualStyleBackColor = true;
      b.MouseUp += new System.Windows.Forms.MouseEventHandler(managedButton_MouseUp);
      b.GotFocus += new EventHandler(shared_GotFocus);
      this.Controls.Add(b);
    }

    private void AdjustForm()
    {
      if (_adjustingForm) return;
      if (_opt == null) return;

      int bc = 0;
      foreach (Control c in this.Controls)
        if (c.GetType() == typeof(Button)) bc++;

      if (bc < 1) return;
      _adjustingForm = true; //prevent us from stepping on our own toes

      //difference between form and client area
      int adjW = this.Width - this.ClientRectangle.Width;
      int adjH = this.Height - this.ClientRectangle.Height;

      int cw = this.ClientRectangle.Width;
      if (cw < MINBUTTONWIDTH)
      {
        cw = MINBUTTONWIDTH;
        this.Width = cw + adjW;
      }

      //since we want perfect fit, we will always reset the size based on the number of buttons
      int ch = this.ClientRectangle.Height / bc + (bc * SpacerY); //first guess

      //if the last button count was 0, both 'last' values are not useful
      if (_lastButtonCount == 0)
      {
        _lastButtonCount = bc;
        _lastButtonHeight = ch;
      }

      //if the number of buttons has changed, try to reuse the button height
      if(_lastButtonCount != bc) ch = _lastButtonHeight;

      if (ch < MINBUTTONHEIGHT) ch = MINBUTTONHEIGHT;

      //force exact height base new calculated height (ch)
      this.Height = bc * (ch + SpacerY) + adjH;

      //save to try and reuse if the button count changes
      _lastButtonHeight = ch;
      _lastButtonCount = bc;

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

      _adjustingForm = false;
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
      if (_adjustingForm) return; //don't react to my own resizes
      if (_opt == null) return; //don't do anything if settings have not been loaded
      AdjustForm();
    }
  }
}
  