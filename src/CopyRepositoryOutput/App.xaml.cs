using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace CopyRepositoryOutput
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private bool mDeleteOnExit = false;
    private string mExecuteablePath;

    public App()
    {
      mExecuteablePath = Assembly.GetExecutingAssembly().Location;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
#if DEBUG
      base.OnStartup(e);
#else
      var args = e.Args;
      if (args.Length == 0)
      {
        // duplicate myself to a temporary directory. Then, restart that application
        var source = mExecuteablePath;
        var dest = Path.ChangeExtension(Path.GetTempFileName(), "exe");
        var bytes = File.ReadAllBytes(source);
        File.WriteAllBytes(dest, bytes);
        Process.Start(dest, CreateMD5Hash(bytes));
        Shutdown();
        return;
      }
      else
      {
        mDeleteOnExit = true;
        base.OnStartup(e);
      }
#endif
    }

    protected override void OnExit(ExitEventArgs e)
    {
      base.OnExit(e);
      if (mDeleteOnExit)
      {
        DeleteMyself();
      }
    }

    private string CreateMD5Hash(byte[] bytes)
    {
      using (var md5 = MD5.Create())
      {
        return Convert.ToBase64String(md5.ComputeHash(bytes));
      }
    }

    private void DeleteMyself()
    {
      var info = new ProcessStartInfo();
      info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + mExecuteablePath;
      info.WindowStyle = ProcessWindowStyle.Hidden;
      info.CreateNoWindow = true;
      info.FileName = "cmd.exe";
      Process.Start(info);
    }
  }
}
