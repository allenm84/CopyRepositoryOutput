using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public static class EnvironmentVars
  {
    const string Name = "PATH";

    public static bool AddToPath(string path)
    {
      try
      {
        var attr = File.GetAttributes(path);
        if (!attr.HasFlag(FileAttributes.Directory))
        {
          AppLogger.WriteLine("The path provided is not a directory");
          return false;
        }
      }
      catch (Exception ex)
      {
        AppLogger.WriteLine("Error retrieving attributes: {0}", ex.Message);
        return false;
      }

      var paths = Environment
        .GetEnvironmentVariable(Name, EnvironmentVariableTarget.Machine)
        .Split(';')
        .ToList();

      var exists = paths
        .Select(p => Path.GetFullPath(p))
        .Any(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));

      if (exists)
      {
        AppLogger.WriteLine("The directory is already in the PATH variable");
        return false;
      }
      else
      {
        paths.Add(path);
        try
        {
          Environment.SetEnvironmentVariable(Name, string.Join(";", paths), EnvironmentVariableTarget.Machine);
          AppLogger.WriteLine("The directory was added to the PATH variable");
        }
        catch (Exception ex)
        {
          AppLogger.WriteLine("Error setting variable name: ", ex.Message);
          return false;
        }
      }

      return true;
    }
  }
}
