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

    public OptionsForm(ref TouchMenuOptions Options)
    {
      InitializeComponent();
      _action = OptionActions.Update;
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
      _action = OptionActions.Exit;
      this.Close();
    }
  }
}
