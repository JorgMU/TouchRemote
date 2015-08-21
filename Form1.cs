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

    private const int BaseX = 0;
    private const int BaseY = 1;
    private const int SpacerY = -1;

    private const int MINBUTTONWIDTH = 48;
    private const int MINBUTTONHEIGHT = 32;

    private bool _adjustingForm = false;

    const int WM_NCRBUTTONDOWN = 0xA4;
    const int WM_NCRBUTTONUP = 0xA5;

    const int WS_THICKFRAME = 0x00040000;
    const int WS_CHILD = 0x40000000;
    const int WS_EX_NOACTIVATE = 0x08000000;
    const int WS_EX_TOOLWINDOW = 0x00000080;

    public Form1()
    {
      InitializeComponent();

      _opt = new TouchMenuOptions();

      ResetButtons();
      
    }

    protected void ResetButtons()
    {
      foreach (Control o in this.Controls)
        if (o.GetType() == typeof(Button))
          this.Controls.Remove(o);

      foreach (string bn in _opt.SelectedSet.Buttons.Keys)
        AddButtonToForm(bn);
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

    private void Form1_Load(object sender, EventArgs e)
    {

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

    private void RemoveButtonFromForm(string Name)
    {
      if (!this.Controls.ContainsKey(Name)) return;
      this.Controls.Remove(this.Controls[Name]);
      AdjustForm();
    }

    private void AddButtonToForm(string Name)
    {
      if (this.Controls.ContainsKey(Name)) RemoveButtonFromForm(Name);
    
      Button b = new Button();
      b.Text = Name;
      b.Name = Name;
      b.Location = new System.Drawing.Point(BaseX, BaseY);
      b.Size = new System.Drawing.Size(MINBUTTONWIDTH, MINBUTTONHEIGHT);
      b.UseVisualStyleBackColor = true;
      b.MouseUp += new System.Windows.Forms.MouseEventHandler(managedButton_MouseUp);
      b.GotFocus += new EventHandler(shared_GotFocus);
      this.Controls.Add(b);

      AdjustForm();

    }

    private void AdjustForm()
    {
      if (_opt == null) return;
      int bc = _opt.ActiveButtonCount;
      if (bc < 1) return;

      _adjustingForm = true;

      int cw = this.ClientRectangle.Width;
      if(cw < MINBUTTONWIDTH)
        this.Width += MINBUTTONWIDTH - cw;
      cw = this.ClientRectangle.Width;

      int ch = this.ClientRectangle.Height / bc;
      if(ch < MINBUTTONHEIGHT)
        this.Height += (MINBUTTONHEIGHT - ch) * bc;
      ch = this.ClientRectangle.Height / bc;

      int i = 0;
      foreach (Control o in this.Controls)
        if(o.GetType().ToString() == "System.Windows.Forms.Button")
        {
          int newY = BaseY + i * ch;
          o.Location = new Point(BaseX, newY);
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
  