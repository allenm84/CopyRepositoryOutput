using System;
using System.Collections.Generic;
using System.Common.References;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CopyRepositoryOutput
{
  public partial class MainForm : BaseForm
  {
    private VistaFolderBrowserDialog dlgFolderBrowse;
    private DataContractFile<CroSettings> dcf;
    private CroSettings settings;

    public MainForm()
    {
      InitializeComponent();
      LoadSettings();
      dlgFolderBrowse = new VistaFolderBrowserDialog();
      dlgFolderBrowse.ShowNewFolderButton = false;
    }

    private void LoadSettings()
    {
      dcf = new DataContractFile<CroSettings>(Environment.SpecialFolder.ApplicationData, "CopyRepositoryOutput", "settings.xml");

      if (!dcf.TryRead(out settings) || settings == null)
      {
        settings = new CroSettings();
      }

      if (string.IsNullOrWhiteSpace(settings.RepositoryPath))
      {
        settings.RepositoryPath = @"D:\Repositories";
      }

      txtRepositoryPath.Text = settings.RepositoryPath;
    }

    private async void ReadRepositories()
    {
      treeRepositories.BeginUpdate();
      treeRepositories.Nodes.Clear();

      var nodes = await Task.Run(() =>
      {
        dcf.TryWrite(settings);

        var dir = new DirectoryInfo(settings.RepositoryPath);
        var list = new List<CroInfoNode>();

        foreach (var repo in dir.EnumerateDirectories())
        {
          var path = Path.Combine(repo.FullName, "cro.xml");
          var info = new CroInfo(path);
          info.Read();

          var node = new CroInfoNode(info, repo.Name);
          list.Add(node);
        }

        return list.ToArray();
      });

      treeRepositories.Nodes.AddRange(nodes);
      treeRepositories.EndUpdate();
    }

    private async void WriteRepositories()
    {
      var values = treeRepositories.Nodes.OfType<CroInfoNode>().Select(n => n.Info).ToArray();
      await Task.Run(() =>
      {
        dcf.TryWrite(settings);

        foreach (var value in values)
        {
          value.Write();
        }
      });
    }

    private void btnRead_Click(object sender, EventArgs e)
    {
      string directory = txtRepositoryPath.Text;
      if (Directory.Exists(directory))
      {
        settings.RepositoryPath = directory;
        ReadRepositories();
      }
    }

    private void btnWrite_Click(object sender, EventArgs e)
    {
      string directory = txtRepositoryPath.Text;
      if (Directory.Exists(directory))
      {
        settings.RepositoryPath = directory;
        WriteRepositories();
      }
    }

    private void treeRepositories_AfterSelect(object sender, TreeViewEventArgs e)
    {
      object value = null;

      var node = e.Node as CroInfoNode;
      if (node != null)
      {
        value = node.Info;
      }

      propertyGrid.SelectedObject = value;
    }

    private void btnRun_Click(object sender, EventArgs e)
    {
      if (treeRepositories.Nodes.Count > 0)
      {
        var values = treeRepositories.Nodes
          .OfType<CroInfoNode>()
          .Select(n => n.Info)
          .ToArray();

        using (var dlg = new RunnerForm(values))
        {
          dlg.ShowDialog(this);
        }
      }
    }
  }
}
