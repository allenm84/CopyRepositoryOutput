using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public static class Enums<T>
  {
    public readonly static T[] Values;
    static Enums()
    {
      Values = (T[])Enum.GetValues(typeof(T));
    }
  }
}
