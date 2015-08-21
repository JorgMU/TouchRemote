using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TouchRemote
{
  
  public partial class OptionsForm : Form
  {
    private bool ready = false;
    private OptionActions _action;
    private List<string> _groups;
    private string _active;

    public OptionActions Action { get { return _action; } }
    public string ActiveGroup {  get { return _active; } }

    public OptionsForm(List<string> Groups, string ActiveGroup)
    {
      _action = OptionActions.Nothing;
      _groups = Groups;
      _active = ActiveGroup;
      InitializeComponent();
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {
      activeCB.Items.Clear();
      foreach (string s in _groups)
        activeCB.Items.Add(s);
      activeCB.SelectedItem = _active;

      //attempt to fit to content
      int fudge = returnButton.Left;
      int padX = Width - ClientRectangle.Width;
      int padY = Height - ClientRectangle.Height;
      Width = padX + 2*fudge + returnButton.Width;
      Height = padY + fudge + returnButton.Top + returnButton.Height;

      ready = true;
    }

    private void exitButton_Click(object sender, EventArgs e)
    {
      _action = OptionActions.Exit;
      this.Close();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!ready) return;
      _active = activeCB.SelectedItem.ToString();
      _action = OptionActions.Update;
      this.Close();
    }

    private void returnButton_Click(object sender, EventArgs e)
    {
      _action = OptionActions.Nothing;
      this.Close();
    }

    private void OptionsForm_ResizeEnd(object sender, EventArgs e)
    {
      this.Text = string.Format("{0},{1}", this.Width, this.Height);
    }
  }
}
