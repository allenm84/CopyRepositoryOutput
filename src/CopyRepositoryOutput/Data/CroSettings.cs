using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  [DataContract(Name = "CroSettings", Namespace = CroSettings.Namespace)]
  public class CroSettings : ExtensibleDataObject
  {
    public const string Namespace = "http://www.michael.com/apps/CopyRepositoryOutput";

    [DataMember(Order = 0)]
    public string RepositoryPath { get; set; }
  }
}
