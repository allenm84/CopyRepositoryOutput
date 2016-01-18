using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyRepositoryOutput
{
  public partial class CroInfoEditor : UserControl
  {
    static uint sCurrentId = 0;

    private CroInfo mInfo = null;
    private DataTable mPatternTable = new DataTable("Patterns");
    private bool mIgnoreCommits = false;

    public CroInfoEditor()
    {
      InitializeComponent();

      cboTypes.DataSource = Enum.GetValues(typeof(CroInfoType))
        .Cast<CroInfoType>()
        .Select(t => new { Value = t, Display = t.ToString() })
        .ToList();
      cboTypes.DisplayMember = "Display";
      cboTypes.ValueMember = "Value";

      mPatternTable.Columns.Add("Text");
      mPatternTable.RowChanged += mPatternTable_RowChanged;
      mPatternTable.RowDeleted += mPatternTable_RowChanged;
      gridPatterns.DataSource = mPatternTable;

      Info = null;
    }

    public CroInfo Info
    {
      get { return mInfo; }
      set 
      {
        Write();
        mInfo = value;
        tableLayoutPanel1.Visible = (mInfo != null);
        Read();
      }
    }

    private void Read()
    {
      mIgnoreCommits = true;

      if (mInfo != null)
      {
        txtPartial.Text = mInfo.Partial;

        mPatternTable.BeginLoadData();
        mPatternTable.Rows.Clear();
        foreach (var pattern in mInfo.Patterns)
        {
          mPatternTable.Rows.Add(pattern);
        }
        mPatternTable.EndLoadData();

        cboTypes.SelectedValue = mInfo.Type;
      }

      mIgnoreCommits = false;
    }

    private void Write()
    {
      if (mInfo != null)
      {
        mInfo.Partial = txtPartial.Text;
        mInfo.Patterns = mPatternTable.Rows.OfType<DataRow>().Select(r => r.Field<string>(0)).ToArray();
        mInfo.Type = (CroInfoType)cboTypes.SelectedValue;
        mInfo.Normalize();
      }
    }

    private async void Commit()
    {
      if (mIgnoreCommits)
      {
        return;
      }

      uint id = ++sCurrentId;
      await Task.Yield();
      if (id != sCurrentId)
      {
        return;
      }

      Write();
      Read();
    }

    private void UpdateControls()
    {
      bool isDefault = (cboTypes.SelectedValue).Equals(CroInfoType.Default);
      txtPartial.Enabled = isDefault;

      gridPatterns.AllowUserToAddRows = isDefault;
      gridPatterns.AllowUserToDeleteRows = isDefault;
      gridPatterns.ReadOnly = !isDefault;
      gridPatterns.BackgroundColor = isDefault ? SystemColors.Window : SystemColors.Control;
      gridPatterns.Enabled = isDefault;
    }

    private void cboTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
      UpdateControls();
      Commit();
    }

    private void txtPartial_TextChanged(object sender, EventArgs e)
    {
      Commit();
    }

    private void mPatternTable_RowChanged(object sender, DataRowChangeEventArgs e)
    {
      Commit();
    }
  }
}
