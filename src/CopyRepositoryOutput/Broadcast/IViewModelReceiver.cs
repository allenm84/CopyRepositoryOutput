using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public interface IViewModelReceiver
  {
    bool Receive(BaseViewModel viewModel);
  }
}
