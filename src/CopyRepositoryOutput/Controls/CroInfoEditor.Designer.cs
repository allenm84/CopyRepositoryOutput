namespace CopyRepositoryOutput
{
  partial class CroInfoEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.cboTypes = new System.Windows.Forms.ComboBox();
      this.txtPartial = new System.Windows.Forms.TextBox();
      this.gridPatterns = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridPatterns)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.cboTypes, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtPartial, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.gridPatterns, 1, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(227, 150);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(18, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(34, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Type:";
      // 
      // label2
      // 
      this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 38);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(39, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Partial:";
      // 
      // label3
      // 
      this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 98);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(49, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Patterns:";
      // 
      // cboTypes
      // 
      this.cboTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.cboTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboTypes.FormattingEnabled = true;
      this.cboTypes.Location = new System.Drawing.Point(58, 4);
      this.cboTypes.Name = "cboTypes";
      this.cboTypes.Size = new System.Drawing.Size(166, 21);
      this.cboTypes.TabIndex = 3;
      this.cboTypes.SelectedIndexChanged += new System.EventHandler(this.cboTypes_SelectedIndexChanged);
      // 
      // txtPartial
      // 
      this.txtPartial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtPartial.Location = new System.Drawing.Point(58, 35);
      this.txtPartial.Name = "txtPartial";
      this.txtPartial.Size = new System.Drawing.Size(166, 20);
      this.txtPartial.TabIndex = 4;
      this.txtPartial.TextChanged += new System.EventHandler(this.txtPartial_TextChanged);
      // 
      // gridPatterns
      // 
      this.gridPatterns.AllowUserToResizeRows = false;
      this.gridPatterns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.gridPatterns.BackgroundColor = System.Drawing.SystemColors.Window;
      this.gridPatterns.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.gridPatterns.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.gridPatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridPatterns.ColumnHeadersVisible = false;
      this.gridPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridPatterns.Location = new System.Drawing.Point(58, 63);
      this.gridPatterns.Name = "gridPatterns";
      this.gridPatterns.RowHeadersVisible = false;
      this.gridPatterns.RowTemplate.Height = 20;
      this.gridPatterns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.gridPatterns.Size = new System.Drawing.Size(166, 84);
      this.gridPatterns.TabIndex = 5;
      // 
      // CroInfoEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "CroInfoEditor";
      this.Size = new System.Drawing.Size(227, 150);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridPatterns)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox cboTypes;
    private System.Windows.Forms.TextBox txtPartial;
    private System.Windows.Forms.DataGridView gridPatterns;
  }
}
