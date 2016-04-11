using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace CopyRepositoryOutput
{
  public sealed class BrowseForDirectoryCommand : BaseCommand
  {
    protected override bool InternalCanExecute(object parameter)
    {
      return parameter is TextBox;
    }

    protected override void InternalExecute(object parameter)
    {
      var textBox = parameter as TextBox;
      var dlg = new VistaFolderBrowserDialog();
      if (dlg.ShowDialog().GetValueOrDefault())
      {
        textBox.Text = dlg.SelectedPath;
      }
    }
  }
}
