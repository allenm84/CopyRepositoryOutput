﻿using System;
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

      Type = CroInfoType.Default;
      Partial = "Programs";
      Patterns = new string[] { "*.exe", "*.dll" };
      Key = null;
      AddToPath = false;
    }

    public string Filepath { get { return mFilepath; } }

    public CroInfoType Type { get; set; }
    public string Partial { get; set; }
    public string[] Patterns { get; set; }
    public string Key { get; set; }
    public bool AddToPath { get; set; }

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
      }

      AddToPath = false;
      Key = null;

      var attrKey = element.Attribute("key");
      if (attrKey != null)
      {
        Key = attrKey.Value;
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

      var attrAddToPath = element.Attribute("addToPath");
      if (attrAddToPath != null && !string.IsNullOrWhiteSpace(attrAddToPath.Value))
      {
        bool value;
        if (bool.TryParse(attrAddToPath.Value, out value))
        {
          AddToPath = value;
        }
      }

      Normalize();
    }

    public void Normalize()
    {
      switch (Type)
      {
        case CroInfoType.Default:
          {
            if (Patterns == null || Patterns.Length == 0 || Patterns.Contains("*.nupkg"))
            {
              Patterns = new string[] { "*.exe", "*.dll" };
            }
            if (string.IsNullOrEmpty(Partial) || Partial == "[nuget]")
            {
              Partial = "Programs";
            }
            break;
          }
        case CroInfoType.Ignore:
          {
            Patterns = new string[0];
            Partial = string.Empty;
            AddToPath = false;
            break;
          }
        case CroInfoType.NuGet:
          {
            Patterns = new string[] { "*.nupkg" };
            Partial = "[nuget]";
            AddToPath = false;
            break;
          }
      }
    }

    public void Write()
    {
      var element = new XElement("cro");
      element.Add(new XAttribute("type", Type));

      if (!string.IsNullOrWhiteSpace(Key))
      {
        element.Add(new XAttribute("key", Key));
      }

      if (Type == CroInfoType.Default)
      {
        element.Add(new XAttribute("path", Partial));
        element.Add(new XAttribute("addToPath", AddToPath));

        foreach (var pattern in Patterns)
        {
          element.Add(new XElement("pattern", pattern));
        }
      }

      File.WriteAllText(mFilepath, element.ToString());
    }
  }
}
