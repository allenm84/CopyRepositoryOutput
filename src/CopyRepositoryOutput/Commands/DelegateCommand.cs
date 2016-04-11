using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  internal class DelegateCommand : BaseCommand
  {
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    public DelegateCommand(Action execute)
      : this(execute, null)
    {

    }

    public DelegateCommand(Action execute, Func<bool> canExecute)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    }

    protected override bool InternalCanExecute(object parameter)
    {
      if (canExecute == null)
      {
        return true;
      }

      return canExecute();
    }

    protected override void InternalExecute(object parameter)
    {
      execute();
    }
  }
}
