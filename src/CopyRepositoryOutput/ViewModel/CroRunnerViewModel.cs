using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  public class CroRunnerViewModel : BaseViewModel
  {
    private readonly SynchronizationContext mSyncContext;
    private readonly BindingList<string> mOutput;

    private readonly DelegateCommand mCloseCommand;
    private readonly CancellationTokenSource mTokenSource;

    public CroRunnerViewModel(CroInfo[] data)
    {
      mSyncContext = SynchronizationContext.Current;
      mOutput = new BindingList<string>();

      mCloseCommand = new DelegateCommand(DoClose);
      mTokenSource = new CancellationTokenSource();

      DoRunWorkAsync(data, mTokenSource.Token);
    }

    public BindingList<string> Output
    {
      get { return mOutput; }
    }

    public ICommand CloseCommand
    {
      get { return mCloseCommand; }
    }

    private void DoClose()
    {
      mTokenSource.Cancel();
      SetCompleted(true);
    }

    private void WriteLine()
    {
      WriteLine(string.Empty);
    }

    private void WriteLine(string format, params object[] args)
    {
      WriteLine(string.Format(format, args));
    }

    private void WriteLine(string text)
    {
      mSyncContext.Send((x) => mOutput.Add(text), null);
    }

    private async void DoRunWorkAsync(CroInfo[] data, CancellationToken token)
    {
      await Task.Run(() => DoRunWork(data, token), token);
    }

    private void DoRunWork(CroInfo[] data, CancellationToken token)
    {
      var dropbox = Dropbox.Location;
      foreach (var cro in data)
      {
        if (token.IsCancellationRequested)
        {
          return;
        }
        cro.Write();

        var info = new FileInfo(cro.Filepath);
        var repoDir = info.Directory;

        var repo = repoDir.FullName;
        WriteLine("=== {0} ===", repoDir.Name);

        var bin = Path.Combine(repo, "bin");
        if (!Directory.Exists(bin))
        {
          WriteLine("{0} does not contain a bin directory", repo);
          WriteLine();
          continue;
        }

        if (cro.Type == CroInfoType.Ignore)
        {
          WriteLine("Ignoring {0}", repo);
          WriteLine();
          continue;
        }

        var repoName = repoDir.Name;

        var destination = Path.Combine(dropbox, cro.Partial, repoName);
        if (!Directory.Exists(destination))
        {
          Directory.CreateDirectory(destination);
        }

        int fileCount = 0;

        WriteLine("Destination: {0}", destination);
        foreach (var pattern in cro.Patterns)
        {
          if (token.IsCancellationRequested)
          {
            return;
          }

          var files = Directory.EnumerateFiles(bin, pattern);
          foreach (var file in files)
          {
            if (token.IsCancellationRequested)
            {
              return;
            }

            if (CopyFileTo(file, destination))
            {
              ++fileCount;
            }
          }
        }

        WriteLine("Copied {0} files", fileCount);
        WriteLine();
      }

      WriteLine();
      WriteLine("=== Completed! ===");
    }

    private bool CopyFileTo(string src, string dir)
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
          WriteLine("\tSkipping {0}", name);
          return false;
        }
      }

      bool copied = false;
      try
      {
        File.Copy(src, dest, true);
        WriteLine("\t{0} => {1}", name, dest);
        copied = true;
      }
      catch (Exception ex)
      {
        WriteLine("\tCouldn't copy {0} because {1}", name, ex.Message);
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
