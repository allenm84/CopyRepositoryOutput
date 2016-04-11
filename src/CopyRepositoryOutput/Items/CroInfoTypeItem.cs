using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public class CroInfoTypeItem : ValueDisplayItem<CroInfoType>
  {
    static readonly Lazy<CroInfoTypeItem[]> sLazyAll;
    static CroInfoTypeItem()
    {
      sLazyAll = new Lazy<CroInfoTypeItem[]>(GetAllItems, true);
    }

    public static CroInfoTypeItem[] All
    {
      get { return sLazyAll.Value; }
    }

    private static CroInfoTypeItem[] GetAllItems()
    {
      return Enums<CroInfoType>.Values
        .Select(t => new CroInfoTypeItem(t))
        .ToArray();
    }

    public CroInfoTypeItem(CroInfoType type)
      : base(type)
    {

    }
  }
}
