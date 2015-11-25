using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CopyRepositoryOutput
{
  public class CroInfo
  {
    private readonly string mFilepath;

    public CroInfo(string filepath)
    {
      mFilepath = filepath;

      Partial = "Programs";
      Patterns = new string[] { "*.exe", "*.dll" };
      Type = CroInfoType.Default;
    }

    [Browsable(false)]
    public string Filepath { get { return mFilepath; } }

    public string Partial { get; set; }
    public string[] Patterns { get; set; }
    public CroInfoType Type { get; set; }

    public void Read()
    {
      if (!File.Exists(mFilepath))
      {
        return;
      }

      var element = XElement.Parse(File.ReadAllText(mFilepath));

      var attrType = element.Attribute("type");
      if (attrType != null)
      {
        Type = (CroInfoType)Enum.Parse(typeof(CroInfoType), attrType.Value, true);
        if (Type != CroInfoType.Default)
        {
          return;
        }
      }

      Patterns = element
        .Descendants("pattern")
        .Select(p => p.Value)
        .ToArray();

      var attrPath = element.Attribute("path");
      if (attrPath != null && !string.IsNullOrWhiteSpace(attrPath.Value))
      {
        Partial = attrPath.Value.Trim();
      }
    }

    public void Write()
    {
      var element = new XElement("cro");
      element.Add(new XAttribute("type", Type));

      if (Type == CroInfoType.Default)
      {
        element.Add(new XAttribute("path", Partial));
        foreach (var pattern in Patterns)
        {
          element.Add(new XElement("pattern", pattern));
        }
      }

      File.WriteAllText(mFilepath, element.ToString());
    }

    public void NormalizeAndWrite()
    {
      if (Type == CroInfoType.NuGet)
      {
        Patterns = new[] { "*.nupkg" };
        Partial = "[nuget]";
      }

      Write();
    }
  }
}
