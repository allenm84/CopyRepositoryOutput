using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
  {
    private class NotifyPropertyChangedDefer : IDisposable
    {
      private WeakReference<BaseNotifyPropertyChanged> reference;

      public NotifyPropertyChangedDefer(BaseNotifyPropertyChanged value)
      {
        reference = new WeakReference<BaseNotifyPropertyChanged>(value);
        value.SuspendNotifications();
      }

      public void Dispose()
      {
        BaseNotifyPropertyChanged value;
        if (reference.TryGetTarget(out value))
        {
          value.ResumeNotifications();
        }
      }
    }

    private bool mAllowPropertyChangedEvents = true;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void SuspendNotifications()
    {
      mAllowPropertyChangedEvents = false;
    }

    protected void ResumeNotifications()
    {
      mAllowPropertyChangedEvents = true;
    }

    protected IDisposable DeferNotifications()
    {
      return new NotifyPropertyChangedDefer(this);
    }

    protected virtual void BeforePropertyChanged(string propertyName)
    {
      // do nothing
    }

    protected virtual void AfterPropertyChanged(string propertyName)
    {
      // do nothing
    }

    protected void FirePropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (!mAllowPropertyChangedEvents)
      {
        return;
      }

      BeforePropertyChanged(propertyName);

      var changed = PropertyChanged;
      if (changed != null)
      {
        changed(this, new PropertyChangedEventArgs(propertyName));
      }

      AfterPropertyChanged(propertyName);
    }
  }
}
