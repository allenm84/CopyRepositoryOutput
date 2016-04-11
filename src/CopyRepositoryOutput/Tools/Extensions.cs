using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public static class Extensions
  {
    public static void WriteLine(this IProgress<string> progress)
    {
      progress.Report(string.Empty);
    }

    public static void WriteLine(this IProgress<string> progress, string text)
    {
      progress.Report(text);
    }

    public static void WriteLine(this IProgress<string> progress, string format, params object[] args)
    {
      progress.Report(string.Format(format, args));
    }

    public static IDisposable DeferBinding<T>(this BindingList<T> list, bool notify = true)
    {
      return new BindingListNotifier<T>(list, notify);
    }

    public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> values)
    {
      foreach (T value in values)
      {
        list.Add(value);
      }
    }

    private class BindingListNotifier<T> : IDisposable
    {
      private BindingList<T> list;
      private bool notify;

      public BindingListNotifier(BindingList<T> list, bool notify)
      {
        this.list = list;
        this.notify = notify;
        list.RaiseListChangedEvents = false;
      }

      void IDisposable.Dispose()
      {
        if (list != null)
        {
          list.RaiseListChangedEvents = true;
          if (notify)
          {
            list.ResetBindings();
          }
        }

        list = null;
      }
    }
  }
}
