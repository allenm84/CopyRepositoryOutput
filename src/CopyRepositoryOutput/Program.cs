using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyRepositoryOutput
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        // duplicate myself to a temporary directory. Then, restart that application
        var source = Application.ExecutablePath;
        var dest = Path.ChangeExtension(Path.GetTempFileName(), "exe");
        var bytes = File.ReadAllBytes(source);
        File.WriteAllBytes(dest, bytes);
        Process.Start(dest, CreateMD5Hash(bytes));
      }
      else
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
        DeleteMyself();
      }
    }

    private static string CreateMD5Hash(byte[] bytes)
    {
      using (var md5 = MD5.Create())
      {
        return Convert.ToBase64String(md5.ComputeHash(bytes));
      }
    }

    private static void DeleteMyself()
    {
      var info = new ProcessStartInfo();
      info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath;
      info.WindowStyle = ProcessWindowStyle.Hidden;
      info.CreateNoWindow = true;
      info.FileName = "cmd.exe";
      Process.Start(info); 
    }
  }
}
