using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  public abstract class BaseCommand : ICommand
  {
    public void Refresh()
    {
      var changed = CanExecuteChanged;
      if (changed != null)
      {
        changed(this, EventArgs.Empty);
      }
    }

    public bool CanExecute(object parameter)
    {
      return InternalCanExecute(parameter);
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
      if (InternalCanExecute(parameter))
      {
        InternalExecute(parameter);
      }
    }

    protected abstract bool InternalCanExecute(object parameter);
    protected abstract void InternalExecute(object parameter);
  }
}
