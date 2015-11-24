using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public static class ProgressExtensions
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
  }
}
