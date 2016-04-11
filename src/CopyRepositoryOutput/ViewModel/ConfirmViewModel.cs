using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  public class ConfirmViewModel : BaseViewModel
  {
    private readonly DelegateCommand mAcceptCommand;
    private readonly DelegateCommand mCancelCommand;

    public ConfirmViewModel(string message, string caption = "Confirm")
    {
      mAcceptCommand = new DelegateCommand(DoAccept);
      mCancelCommand = new DelegateCommand(DoCancel);

      Message = message;
      Caption = caption;
    }

    public string Message
    {
      get { return GetField<string>(); }
      private set { SetField(value); }
    }

    public string Caption
    {
      get { return GetField<string>(); }
      private set { SetField(value); }
    }

    public ICommand AcceptCommand
    {
      get { return mAcceptCommand; }
    }

    public ICommand CancelCommand
    {
      get { return mCancelCommand; }
    }

    private void DoCancel()
    {
      SetCompleted(false);
    }

    private void DoAccept()
    {
      SetCompleted(true);
    }
  }
}
