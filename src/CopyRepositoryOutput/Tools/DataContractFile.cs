using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CopyRepositoryOutput
{
  public class DataContractFile<T> where T : class
  {
    private readonly DataContractSerializer dcs;
    private readonly string filepath;

    static string CreateDirectory(Environment.SpecialFolder folder, string directoryName, string filename)
    {
      string folderPath = Environment.GetFolderPath(folder);
      string directory = Path.Combine(folderPath, directoryName);

      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      return Path.Combine(directory, filename);
    }

    public DataContractFile(Environment.SpecialFolder folder, string directoryName, string filename) :
      this(CreateDirectory(folder, directoryName, filename))
    {

    }

    public DataContractFile(string filepath)
    {
      this.filepath = filepath;
      dcs = new DataContractSerializer(typeof(T));
    }

    public T Read()
    {
      if (!File.Exists(filepath))
      {
        return default(T);
      }

      using (var stream = File.OpenRead(filepath))
      {
        return dcs.ReadObject(stream) as T;
      }
    }

    public bool TryRead(out T value)
    {
      bool success = true;

      value = default(T);
      try { value = Read(); }
      catch { success = false; }

      return success;
    }

    public void Write(T value)
    {
      using (var stream = File.Create(filepath))
      {
        dcs.WriteObject(stream, value);
      }
    }

    public bool TryWrite(T value)
    {
      bool success = true;

      try { Write(value); }
      catch { success = false; }

      return success;
    }

    public byte[] Serialize(T value)
    {
      using (var stream = new MemoryStream())
      {
        dcs.WriteObject(stream, value);
        return stream.ToArray();
      }
    }
  }
}
