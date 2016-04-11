using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;

namespace CopyRepositoryOutput
{
  public static class FileSystem
  {
    static readonly char[] sInvalidPathCharacters;
    static readonly BrowseForDirectoryCommand sBrowseForDirectoryCommand;

    static FileSystem()
    {
      sInvalidPathCharacters = Path.GetInvalidPathChars();
      sBrowseForDirectoryCommand = new BrowseForDirectoryCommand();
    }

    public static ICommand BrowseForDirectoryCommand
    {
      get { return sBrowseForDirectoryCommand; }
    }

    public static bool ContainsInvalidCharacters(string pathString)
    {
      return pathString.IndexOfAny(sInvalidPathCharacters) > -1;
    }

    public static bool IsValidDirectory(string pathString)
    {
      if (string.IsNullOrWhiteSpace(pathString))
      {
        return false;
      }

      if (ContainsInvalidCharacters(pathString))
      {
        return false;
      }

      return Directory.Exists(pathString);
    }
  }
}
