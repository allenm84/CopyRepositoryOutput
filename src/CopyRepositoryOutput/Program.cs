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
      var path = Application.StartupPath;
      var dropbox = Dropbox.Location;

      var repositories = Directory.EnumerateDirectories(path);
      foreach (var repo in repositories)
      {
        Console.WriteLine("=== {0} ===", Path.GetFileNameWithoutExtension(repo));

        var bin = Path.Combine(repo, "bin");
        if (!Directory.Exists(bin))
        {
          Console.WriteLine("{0} does not contain a bin directory", repo);
          Console.WriteLine();
          continue;
        }

        var partial = "Programs";
        var patterns = new string[] { "*.exe", "*.dll" };
        var ignore = false;

        var config = Path.Combine(repo, "cro.xml");
        if (File.Exists(config))
        {
          var element = XElement.Parse(File.ReadAllText(config));
          ReadConfig(element, 
            ref partial, 
            ref patterns, 
            ref ignore);
        }

        if (ignore)
        {
          Console.WriteLine("Ignoring {0}", repo);
          Console.WriteLine();
          continue;
        }

        var repoName = new DirectoryInfo(repo).Name;

        var destination = Path.Combine(dropbox, partial, repoName);
        if (!Directory.Exists(destination))
        {
          Directory.CreateDirectory(destination);
        }

        int files = 0;

        Console.WriteLine("Destination: {0}", destination);
        foreach (var pattern in patterns)
        {
          var values = Directory.EnumerateFiles(bin, pattern);
          foreach (var value in values)
          {
            if(CopyFileTo(value, destination))
            {
            ++files;
            }
          }
        }

        Console.WriteLine("Copied {0} files", files);
        Console.WriteLine();
      }

      Console.WriteLine();
      Console.Write("Press any key to continue...");
      Console.Read();
    }

    private static void ReadConfig(XElement element, ref string partial, ref string[] patterns, ref bool ignore)
    {
      var attrIgnore = element.Attribute("ignore");
      if (attrIgnore != null && (attrIgnore.Value == "yes" || attrIgnore.Value == "true"))
      {
        ignore = true;
        return;
      }

      var attrType = element.Attribute("type");
      if (attrType != null && attrType.Value == "nuget")
      {
        patterns = new[] { "*.nupkg" };
        partial = "[nuget]";
        return;
      }

      var attrPatterns = element.Attribute("patterns");
      if (attrPatterns != null && !string.IsNullOrWhiteSpace(attrPatterns.Value))
      {
        patterns = attrPatterns.Value
          .Split(';')
          .Select(p => p.Trim())
          .ToArray();
      }

      var attrPath = element.Attribute("path");
      if (attrPath != null && !string.IsNullOrWhiteSpace(attrPath.Value))
      {
        partial = attrPath.Value.Trim();
      }
    }

    private static bool CopyFileTo(string src, string dir)
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
          Console.WriteLine("\tSkipping {0}", name);
          return false;
        }
      }

      Console.WriteLine("\t{0} => {1}", name, dest);
      File.Copy(src, dest, true);
      return true;
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
