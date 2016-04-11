using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CopyRepositoryOutput.Windows;

namespace CopyRepositoryOutput
{
  public class CroProvider : IViewModelReceiver
  {
    private readonly CroViewModel mViewModel;

    public CroProvider()
    {
      mViewModel = new CroViewModel();
      ViewModelBroadcaster.Instance.Add(this);
    }

    public CroViewModel ViewModel
    {
      get { return mViewModel; }
    }

    private async void ShowMessageBox(ConfirmViewModel confirm)
    {
      await Task.Yield();
      try
      {
        var result = MessageBox.Show(confirm.Message, confirm.Caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        var command = (result == MessageBoxResult.Yes)
          ? confirm.AcceptCommand
          : confirm.CancelCommand;
        if (command.CanExecute(this))
        {
          command.Execute(this);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    private async void ShowRunnerView(CroRunnerViewModel runner)
    {
      await Task.Yield();
      try
      {
        var view = new CroRunnerView();
        view.DataContext = runner;
        view.ShowDialog();
        runner.CloseCommand.Execute(this);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    bool IViewModelReceiver.Receive(BaseViewModel viewModel)
    {
      var confirm = viewModel as ConfirmViewModel;
      if (confirm != null)
      {
        ShowMessageBox(confirm);
        return true;
      }

      var runner = viewModel as CroRunnerViewModel;
      if (runner != null)
      {
        ShowRunnerView(runner);
        return true;
      }

      return false;
    }
  }
}
