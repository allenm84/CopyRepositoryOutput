using System;
using System.Collections.Generic;
using System.Common.References;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CopyRepositoryOutput
{
  class Program
  {
    static void Main(string[] args)
    {
      string path = Application.StartupPath;
      string dropbox = Dropbox.Location;

      var repositories = Directory.EnumerateDirectories(path);
      foreach (var repo in repositories)
      {
        string ignore = Path.Combine(repo, ".ignore");
        if (File.Exists(ignore))
        {
          continue;
        }

        var bin = Path.Combine(repo, "bin");
        if (!Directory.Exists(bin))
        {
          Console.WriteLine("{0} does not contain a bin directory", repo);
          continue;
        }

        string partial = "Programs";

        var config = Path.Combine(repo, "cro.txt");
        if (File.Exists(config))
        {
          partial = File.ReadAllText(config).Trim();
        }

        var repoName = new DirectoryInfo(repo).Name;

        string destination = Path.Combine(dropbox, partial, repoName);
        if (!Directory.Exists(destination))
        {
          Directory.CreateDirectory(destination);
        }

        Console.WriteLine("Destination: {0}", destination);

        var exes = Directory.EnumerateFiles(bin, "*.exe");
        foreach (var exe in exes)
        {
          CopyFileTo(exe, destination);
        }

        var dlls = Directory.EnumerateFiles(bin, "*.dll");
        foreach (var dll in dlls)
        {
          CopyFileTo(dll, destination);
        }
      }

      Console.WriteLine();
      Console.Write("Press any key to continue...");
      Console.Read();
    }

    private static void CopyFileTo(string src, string dir)
    {
      string name = Path.GetFileName(src);
      if (name.EndsWith("vshost.exe"))
      {
        return;
      }

      string dest = Path.Combine(dir, name);
      if (File.Exists(dest))
      {
        var key1 = GetKey(src);
        var key2 = GetKey(dest);
        if (KeysAreEqual(key1, key2))
        {
          Console.Write("\tSkipping {0}", name);
          return;
        }
      }

      Console.WriteLine("\t{0} => {1}", name, dest);
      File.Copy(src, dest, true);
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
