using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public abstract class ValueDisplayItem<T>
  {
    private readonly T mValue;
    private readonly string mDisplay;

    public ValueDisplayItem(T value)
      : this(value, value.ToString())
    {

    }

    public ValueDisplayItem(T value, string display)
    {
      mValue = value;
      mDisplay = display;
    }

    public T Value
    {
      get { return mValue; }
    }

    public string Display
    {
      get { return mDisplay; }
    }

    public override string ToString()
    {
      return mDisplay;
    }
  }
}
