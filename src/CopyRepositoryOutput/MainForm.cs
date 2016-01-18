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
      CroInfo value = null;

      var node = e.Node as CroInfoNode;
      if (node != null)
      {
        value = node.Info;
      }

      croInfoEditor1.Info = value;
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

    private void splitContainer1_Paint(object sender, PaintEventArgs e)
    {
      var control = sender as SplitContainer;
      //paint the three dots'
      Point[] points = new Point[3];
      var w = control.Width;
      var h = control.Height;
      var d = control.SplitterDistance;
      var sW = control.SplitterWidth;

      //calculate the position of the points'
      if (control.Orientation == Orientation.Horizontal)
      {
        points[0] = new Point((w / 2), d + (sW / 2));
        points[1] = new Point(points[0].X - 10, points[0].Y);
        points[2] = new Point(points[0].X + 10, points[0].Y);
      }
      else
      {
        points[0] = new Point(d + (sW / 2), (h / 2));
        points[1] = new Point(points[0].X, points[0].Y - 10);
        points[2] = new Point(points[0].X, points[0].Y + 10);
      }

      foreach (Point p in points)
      {
        p.Offset(-2, -2);
        e.Graphics.FillEllipse(SystemBrushes.ControlDark,
            new Rectangle(p, new Size(3, 3)));

        p.Offset(1, 1);
        e.Graphics.FillEllipse(SystemBrushes.ControlLight,
            new Rectangle(p, new Size(3, 3)));
      }
    }
  }
}
