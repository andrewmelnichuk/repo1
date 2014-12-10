using System.IO;

namespace Common
{
  public interface ISerializable
  {
    void Write(BinaryWriter writer);
    void Read(BinaryReader reader);
  }
}
