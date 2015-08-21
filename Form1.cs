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
  public enum OptionActions { Update, Exit }

  public partial class Form1 : Form
  {
    private TouchMenuOptions _opt;

    private const int ButtonRootX = 0;
    private const int ButtonRootY = 1;
    private const int ButtonStepY = -1;
    private int _buttonSize;

    const int WM_NCRBUTTONDOWN = 0xA4;
    const int WM_NCRBUTTONUP = 0xA5;

    const int WS_THICKFRAME = 0x00040000;
    const int WS_CHILD = 0x40000000;
    const int WS_EX_NOACTIVATE = 0x08000000;
    const int WS_EX_TOOLWINDOW = 0x00000080;

    public Form1()
    {
      InitializeComponent();

      _buttonSize = this.ClientRectangle.Width;

      _opt = new TouchMenuOptions();
      
      foreach(string bn in _opt.SelectedSet.Buttons.Keys)
      {
        AddButtonToForm(bn);
      }
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
      MessageBox.Show(b.Name + " | " + b.Type.ToString());

      switch (b.Type)
      {
        case ButtonType.Cut:
          SendKeys("^X");
          break;
        case ButtonType.Copy:
          SendKeys("^C");
          break;
        case ButtonType.Paste:
          SendKeys("^V");
          break;
        case ButtonType.Keystroke:
          break;
        default:
          break;
      }
    }

    private void RemoveButtonFromForm(string Name)
    {
      if(!this.Controls.ContainsKey(Name)) return;
      this.Controls.Remove(this.Controls[Name]);
      AdjustForm();
    }

    private void AddButtonToForm(string Name)
    {
      if (this.Controls.ContainsKey(Name)) RemoveButtonFromForm(Name);

      Button b = new Button();
      b.Text = Name;
      b.Name = Name;
      b.Location = new System.Drawing.Point(ButtonRootX, ButtonRootY);
      b.Size = new System.Drawing.Size(_buttonSize, _buttonSize);
      b.UseVisualStyleBackColor = true;
      b.MouseUp += new System.Windows.Forms.MouseEventHandler(managedButton_MouseUp);
      b.GotFocus += new EventHandler(shared_GotFocus);
      this.Controls.Add(b);

      AdjustForm();
    }

    private void AdjustForm()
    {
      int count = 0;
      foreach(object o in this.Controls)
        if(o.GetType().ToString() == "System.Windows.Forms.Button")
        {
          Button b = (Button)o;
          int newY = ButtonRootY + count * (_buttonSize + ButtonStepY);
          b.Location = new Point(ButtonRootX, newY);
          b.TabIndex = count;
          count++;
        }

      if (count < 1) return;

      int newCH = 1 + ButtonRootY + count * (_buttonSize + ButtonStepY);

      if(newCH > this.ClientRectangle.Height)
        while (newCH > this.ClientRectangle.Height)
          this.Height += 1;
      else
        while (newCH < this.ClientRectangle.Height)
          this.Height -= 1;

    }
  }
}
  