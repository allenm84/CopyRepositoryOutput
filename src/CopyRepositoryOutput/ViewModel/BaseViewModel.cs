using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public abstract class BaseViewModel : BaseNotifyPropertyChanged
  {
    private readonly Dictionary<string, object> mFields;
    private readonly TaskCompletionSource<bool> mTaskSource;

    public BaseViewModel()
    {
      mFields = new Dictionary<string, object>();
      mTaskSource = new TaskCompletionSource<bool>();
    }

    internal Task<bool> Completed
    {
      get { return mTaskSource.Task; }
    }

    protected void SetCompleted(bool result)
    {
      mTaskSource.TrySetResult(result);
    }

    internal bool Send(BaseViewModel viewModel)
    {
      return ViewModelBroadcaster.Instance.Send(viewModel);
    }

    internal async Task<bool> SendAndWait(BaseViewModel viewModel)
    {
      return Send(viewModel) && await viewModel.Completed;
    }

    protected void Push(IDictionary<string, object> values)
    {
      foreach (var kvp in values)
      {
        PushValue(kvp.Key, kvp.Value);
      }
    }

    protected ReadOnlyDictionary<string, object> Pull()
    {
      return new ReadOnlyDictionary<string, object>(mFields);
    }

    protected bool TryGetValue(string key, out object value)
    {
      return mFields.TryGetValue(key, out value);
    }

    protected T GetField<T>([CallerMemberName] string key = "")
    {
      object value;
      if (!mFields.TryGetValue(key, out value))
      {
        value = default(T);
        mFields[key] = value;
      }
      return (T)value;
    }

    protected void SetField<T>(T value, [CallerMemberName] string key = "", bool force = true)
    {
      if (force || ValueChanged(key, value))
      {
        PushValue(key, value);
      }
    }

    private bool ValueChanged<T>(string key, T value)
    {
      return !(GetField<T>(key).Equals(value));
    }

    private void PushValue(string key, object value)
    {
      mFields[key] = value;
      FirePropertyChanged(key);
    }
  }
}
