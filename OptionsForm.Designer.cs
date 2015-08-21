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
      this.exitButton = new System.Windows.Forms.Button();
      this.activeCB = new System.Windows.Forms.ComboBox();
      this.returnButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // exitButton
      // 
      this.exitButton.Location = new System.Drawing.Point(12, 54);
      this.exitButton.Name = "exitButton";
      this.exitButton.Size = new System.Drawing.Size(128, 36);
      this.exitButton.TabIndex = 0;
      this.exitButton.Text = "Exit";
      this.exitButton.UseVisualStyleBackColor = true;
      this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
      // 
      // activeCB
      // 
      this.activeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.activeCB.FormattingEnabled = true;
      this.activeCB.Location = new System.Drawing.Point(14, 12);
      this.activeCB.Name = "activeCB";
      this.activeCB.Size = new System.Drawing.Size(124, 28);
      this.activeCB.TabIndex = 1;
      this.activeCB.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // returnButton
      // 
      this.returnButton.Location = new System.Drawing.Point(12, 102);
      this.returnButton.Name = "returnButton";
      this.returnButton.Size = new System.Drawing.Size(128, 36);
      this.returnButton.TabIndex = 0;
      this.returnButton.Text = "Return";
      this.returnButton.UseVisualStyleBackColor = true;
      this.returnButton.Click += new System.EventHandler(this.returnButton_Click);
      // 
      // OptionsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(152, 151);
      this.ControlBox = false;
      this.Controls.Add(this.activeCB);
      this.Controls.Add(this.returnButton);
      this.Controls.Add(this.exitButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionsForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "OptionsForm";
      this.Load += new System.EventHandler(this.OptionsForm_Load);
      this.ResizeEnd += new System.EventHandler(this.OptionsForm_ResizeEnd);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button exitButton;
    private System.Windows.Forms.ComboBox activeCB;
    private System.Windows.Forms.Button returnButton;
  }
}