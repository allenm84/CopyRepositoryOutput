using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyRepositoryOutput
{
  public class CroInfoNode : TreeNode
  {
    public CroInfoNode(CroInfo info, string name)
      : base(name)
    {
      Info = info;
    }

    public CroInfo Info { get; private set; }
  }
}
