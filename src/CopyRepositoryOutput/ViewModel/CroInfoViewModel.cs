using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CopyRepositoryOutput
{
  public class CroInfoViewModel : BaseViewModel
  {
    static readonly BindingList<CroPatternViewModel> sEmptyPatterns = new BindingList<CroPatternViewModel>();

    private readonly CroInfo mInfo;
    private readonly BindingList<CroInfoTypeItem> mTypes;
    private readonly BindingList<CroPatternViewModel> mPatterns;

    private readonly DelegateCommand mAddNewPatternCommand;
    private readonly DelegateCommand mSetPatternTextCommand;

    private CroPatternViewModel mSelectedPattern;

    public CroInfoViewModel(string name, CroInfo info)
    {
      mInfo = info;

      mTypes = new BindingList<CroInfoTypeItem>();
      mTypes.AddRange(CroInfoTypeItem.All);

      mPatterns = new BindingList<CroPatternViewModel>();
      LoadPatterns();

      mAddNewPatternCommand = new DelegateCommand(DoAddNewPattern, CanAddNewPattern);
      mSetPatternTextCommand = new DelegateCommand(DoSetPatternText, CanSetPatternText);

      Name = name;
      UpdateIsEditable();
    }

    public string Name
    {
      get { return GetField<string>(); }
      private set { SetField(value); }
    }

    public BindingList<CroInfoTypeItem> Types
    {
      get { return mTypes; }
    }

    public CroInfoType Type
    {
      get { return mInfo.Type; }
      set
      {
        bool normalize = false;
        if (mInfo.Type == CroInfoType.Ignore && value != CroInfoType.Ignore)
        {
          normalize = true;
        }

        mInfo.Type = value;
        FirePropertyChanged();

        if (normalize)
        {
          Normalize();
        }
      }
    }

    public bool IsEditable
    {
      get { return GetField<bool>(); }
      private set { SetField(value); }
    }

    public string Partial
    {
      get { return GetPartial(); }
      set
      {
        mInfo.Partial = value;
        FirePropertyChanged();
      }
    }

    public string EditPatternText
    {
      get { return GetField<string>(); }
      set { SetField(value); }
    }

    public CroPatternViewModel SelectedPattern
    {
      get { return mSelectedPattern; }
      set
      {
        mSelectedPattern = value;
        FirePropertyChanged();
        if (mSelectedPattern != null)
        {
          EditPatternText = mSelectedPattern.Value;
        }
      }
    }

    public ICommand AddNewPatternCommand
    {
      get { return mAddNewPatternCommand; }
    }

    public ICommand SetPatternTextCommand
    {
      get { return mSetPatternTextCommand; }
    }

    public BindingList<CroPatternViewModel> Patterns
    {
      get { return GetPatterns(); }
      private set { FirePropertyChanged(); }
    }

    private bool CanAddNewPattern()
    {
      string text = EditPatternText;
      return
        IsEditable &&
        !string.IsNullOrWhiteSpace(text) && 
        text.StartsWith("*.");
    }

    private void DoAddNewPattern()
    {
      var pattern = new CroPatternViewModel(mPatterns, EditPatternText);
      EditPatternText = string.Empty;
      mPatterns.Add(pattern);
      SelectedPattern = pattern;
    }

    private bool CanSetPatternText()
    {
      string text = EditPatternText;
      return
        IsEditable &&
        !string.IsNullOrWhiteSpace(text) && 
        text.StartsWith("*.") && 
        mSelectedPattern != null;
    }

    private void DoSetPatternText()
    {
      mSelectedPattern.Value = EditPatternText;
    }

    private void Normalize()
    {
      mInfo.Normalize();
      LoadPatterns();
    }

    private void LoadPatterns()
    {
      using (mPatterns.DeferBinding())
      {
        mPatterns.Clear();
        mPatterns.AddRange(mInfo.Patterns.Select(p => new CroPatternViewModel(mPatterns, p)));
      }
    }

    private void UpdateIsEditable()
    {
      IsEditable = (mInfo.Type == CroInfoType.Default);
    }

    private string GetPartial()
    {
      if (IsEditable)
      {
        return mInfo.Partial;
      }
      else
      {
        return string.Empty;
      }
    }

    private BindingList<CroPatternViewModel> GetPatterns()
    {
      if (IsEditable)
      {
        return mPatterns;
      }
      else
      {
        return sEmptyPatterns;
      }
    }

    private void RefreshCommands()
    {
      mAddNewPatternCommand.Refresh();
      mSetPatternTextCommand.Refresh();
    }

    internal void Write()
    {
      mInfo.Patterns = mPatterns
        .OrderBy(p => p.Value)
        .Select(p => p.Value)
        .Distinct(StringComparer.InvariantCultureIgnoreCase)
        .ToArray();

      mInfo.Normalize();
      mInfo.Write();
    }

    internal CroInfo GetInfo()
    {
      Write();
      return mInfo;
    }

    protected override void AfterPropertyChanged(string propertyName)
    {
      base.AfterPropertyChanged(propertyName);

      if (propertyName == nameof(IsEditable))
      {
        FirePropertyChanged(nameof(Partial));
        FirePropertyChanged(nameof(Patterns));
      }
      else if (propertyName == nameof(Type))
      {
        UpdateIsEditable();
      }

      RefreshCommands();
    }
  }
}
