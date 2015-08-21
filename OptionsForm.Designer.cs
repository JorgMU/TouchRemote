namespace TouchRemote
{
  partial class OptionsForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.closeButton = new System.Windows.Forms.Button();
      this.activeCB = new System.Windows.Forms.ComboBox();
      this.updateButton = new System.Windows.Forms.Button();
      this.saveButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // closeButton
      // 
      this.closeButton.Location = new System.Drawing.Point(12, 161);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(254, 36);
      this.closeButton.TabIndex = 0;
      this.closeButton.Text = "Close TouchRemote";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // activeCB
      // 
      this.activeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.activeCB.FormattingEnabled = true;
      this.activeCB.Location = new System.Drawing.Point(12, 12);
      this.activeCB.Name = "activeCB";
      this.activeCB.Size = new System.Drawing.Size(254, 28);
      this.activeCB.TabIndex = 1;
      this.activeCB.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // updateButton
      // 
      this.updateButton.Location = new System.Drawing.Point(12, 56);
      this.updateButton.Name = "updateButton";
      this.updateButton.Size = new System.Drawing.Size(254, 36);
      this.updateButton.TabIndex = 0;
      this.updateButton.Text = "Update";
      this.updateButton.UseVisualStyleBackColor = true;
      this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
      // 
      // saveButton
      // 
      this.saveButton.Location = new System.Drawing.Point(12, 108);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new System.Drawing.Size(254, 36);
      this.saveButton.TabIndex = 0;
      this.saveButton.Text = "Save";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
      // 
      // OptionsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(278, 217);
      this.Controls.Add(this.saveButton);
      this.Controls.Add(this.updateButton);
      this.Controls.Add(this.activeCB);
      this.Controls.Add(this.closeButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionsForm";
      this.Text = "OptionsForm";
      this.Load += new System.EventHandler(this.OptionsForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button closeButton;
    private System.Windows.Forms.ComboBox activeCB;
    private System.Windows.Forms.Button updateButton;
    private System.Windows.Forms.Button saveButton;
  }
}