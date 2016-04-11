using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  public class CroViewModel : BaseViewModel
  {
    private readonly DataContractFile<CroSettings> dcf;
    private readonly CroSettings settings;

    private readonly BindingList<CroInfoViewModel> mItems;

    private readonly DelegateCommand mReadCommand;
    private readonly DelegateCommand mWriteCommand;
    private readonly DelegateCommand mRunCommand;

    public CroViewModel()
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

      mItems = new BindingList<CroInfoViewModel>();

      mReadCommand = new DelegateCommand(DoRead, CanRead);
      mWriteCommand = new DelegateCommand(DoWrite, CanWrite);
      mRunCommand = new DelegateCommand(DoRun, CanRun);

      IsEnabled = true;
    }

    public bool IsEnabled
    {
      get { return GetField<bool>(); }
      private set { SetField(value); }
    }

    public string RepositoriesDirectory
    {
      get { return settings.RepositoryPath; }
      set 
      { 
        settings.RepositoryPath = value;
        FirePropertyChanged();
      }
    }

    public BindingList<CroInfoViewModel> Items
    {
      get { return mItems; }
    }

    public ICommand ReadCommand
    {
      get { return mReadCommand; }
    }

    public ICommand WriteCommand
    {
      get { return mWriteCommand; }
    }

    public ICommand RunCommand
    {
      get { return mRunCommand; }
    }

    private bool CanRead()
    {
      return IsEnabled && FileSystem.IsValidDirectory(RepositoriesDirectory);
    }

    private void DoRead()
    {
      dcf.TryWrite(settings);

      using (mItems.DeferBinding())
      {
        mItems.Clear();

        var dir = new DirectoryInfo(settings.RepositoryPath);
        foreach (var repo in dir.EnumerateDirectories())
        {
          var path = Path.Combine(repo.FullName, "cro.xml");
          var info = new CroInfo(path);
          info.Read();
          mItems.Add(new CroInfoViewModel(repo.Name, info));
        }
      }

      RefreshCommands();
    }

    private bool CanWrite()
    {
      return
        IsEnabled &&
        FileSystem.IsValidDirectory(RepositoriesDirectory) && 
        mItems.Count > 0;
    }

    private void DoWrite()
    {
      dcf.TryWrite(settings);
      foreach (var item in mItems)
      {
        item.Write();
      }
    }

    private bool CanRun()
    {
      return IsEnabled && mItems.Count > 0;
    }

    private async void DoRun()
    {
      IsEnabled = false;

      var data = mItems.Select(i => i.GetInfo()).ToArray();
      await SendAndWait(new CroRunnerViewModel(data));

      IsEnabled = true;
    }

    private void RefreshCommands()
    {
      mReadCommand.Refresh();
      mWriteCommand.Refresh();
      mRunCommand.Refresh();
    }

    protected override void AfterPropertyChanged(string propertyName)
    {
      base.AfterPropertyChanged(propertyName);
      RefreshCommands();
    }
  }
}
