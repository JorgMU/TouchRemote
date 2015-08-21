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
    private OptionActions _action;
    public OptionActions Action {  get { return _action; } }

    private TouchRemoteOptions _opt;

    public OptionsForm(ref TouchRemoteOptions Options)
    {
      InitializeComponent();
      _action = OptionActions.Nothing;
      _opt = Options;
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {
      activeCB.Items.Clear();
      foreach (string s in _opt.Sets.Keys)
        activeCB.Items.Add(s);
      activeCB.SelectedItem = _opt.SelectedSet.Name;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      _action = OptionActions.Exit;
      this.Close();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      _opt.Select(activeCB.SelectedItem.ToString());
    }

    private void updateButton_Click(object sender, EventArgs e)
    {
      _action = OptionActions.Update;
      this.Close();
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
      _action = OptionActions.Save;
      this.Close();
    }
  }
}
