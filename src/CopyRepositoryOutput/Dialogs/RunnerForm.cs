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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyRepositoryOutput
{
  public partial class RunnerForm : BaseForm
  {
    public RunnerForm(CroInfo[] values)
    {
      InitializeComponent();
      DoCopyRepositoryOutput(values, new Progress<string>(WriteLine));
    }

    private void WriteLine(string text)
    {
      txtOutput.AppendText(text + Environment.NewLine);
      txtOutput.ScrollToCaret();
    }

    private async void DoCopyRepositoryOutput(CroInfo[] data, IProgress<string> output)
    {
      await Task.Yield();
      await Task.Run(() =>
      {
        var path = Application.StartupPath;
        var dropbox = Dropbox.Location;

        foreach (var cro in data)
        {
          var info = new FileInfo(cro.Filepath);
          var repoDir = info.Directory;

          var repo = repoDir.FullName;
          output.WriteLine("=== {0} ===", repoDir.Name);

          var bin = Path.Combine(repo, "bin");
          if (!Directory.Exists(bin))
          {
            output.WriteLine("{0} does not contain a bin directory", repo);
            output.WriteLine();
            continue;
          }

          if (cro.Type == CroInfoType.Ignore)
          {
            output.WriteLine("Ignoring {0}", repo);
            output.WriteLine();
            continue;
          }

          var repoName = repoDir.Name;

          var destination = Path.Combine(dropbox, cro.Partial, repoName);
          if (!Directory.Exists(destination))
          {
            Directory.CreateDirectory(destination);
          }

          int fileCount = 0;

          output.WriteLine("Destination: {0}", destination);
          foreach (var pattern in cro.Patterns)
          {
            var files = Directory.EnumerateFiles(bin, pattern);
            foreach (var file in files)
            {
              if (CopyFileTo(file, destination, output))
              {
                ++fileCount;
              }
            }
          }

          output.WriteLine("Copied {0} files", fileCount);
          output.WriteLine();
        }

        output.WriteLine();
        output.WriteLine("=== Completed! ===");
      });
    }

    private static bool CopyFileTo(string src, string dir, IProgress<string> output)
    {
      string name = Path.GetFileName(src);
      if (name.EndsWith("vshost.exe"))
      {
        return false;
      }

      string dest = Path.Combine(dir, name);
      if (File.Exists(dest))
      {
        var key1 = GetKey(src);
        var key2 = GetKey(dest);
        if (KeysAreEqual(key1, key2))
        {
          output.WriteLine("\tSkipping {0}", name);
          return false;
        }
      }

      bool copied = false;
      try
      {
        File.Copy(src, dest, true);
        output.WriteLine("\t{0} => {1}", name, dest);
        copied = true;
      }
      catch (Exception ex)
      {
        output.WriteLine("\tCouldn't copy {0} because {1}", ex.Message);
      }

      return copied;
    }

    private static bool KeysAreEqual(byte[] key1, byte[] key2)
    {
      if (key1.Length != key2.Length)
        return false;

      int len = key1.Length;
      for (int i = 0; i < len; ++i)
      {
        if (key1[i] != key2[i])
          return false;
      }

      return true;
    }

    private static byte[] GetKey(string filepath)
    {
      using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
      {
        var byteArray = File.ReadAllBytes(filepath);
        return sha1.ComputeHash(byteArray);
      }
    }
  }
}
