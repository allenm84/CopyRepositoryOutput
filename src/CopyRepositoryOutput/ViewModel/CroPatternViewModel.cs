using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  public class CroPatternViewModel : BaseViewModel
  {
    private readonly IList<CroPatternViewModel> mList;
    private readonly DelegateCommand mRemoveCommand;

    public CroPatternViewModel(IList<CroPatternViewModel> list, string value)
    {
      mRemoveCommand = new DelegateCommand(DoRemove);

      mList = list;
      Value = value;
    }

    public string Value
    {
      get { return GetField<string>(); }
      set { SetField(value); }
    }

    public ICommand RemoveCommand
    {
      get { return mRemoveCommand; }
    }

    private async void DoRemove()
    {
      if (await SendAndWait(new ConfirmViewModel("Are you sure you want to delete this pattern?")))
      {
        mList.Remove(this);
      }
    }
  }
}
