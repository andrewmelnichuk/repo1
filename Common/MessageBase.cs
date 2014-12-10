using System;
using System.IO;

namespace Common
{
  public class MessageBase : ISerializable
  {
    public Guid UniqueId { get; private set; }
    public long SequenceId { get; set; }
    public byte[] Data { get; private set; }

    public MessageBase(byte[] data = null)
    {
      SequenceId = 1;
      UniqueId = Guid.NewGuid();
      Data = data;
    }

    public void Write(BinaryWriter writer)
    {
      //writer.Write(UniqueId.ToByteArray());
      writer.Write(SequenceId);
      
      //writer.Write(Data.Length);
      //writer.Write(Data);
    }

    public void Read(BinaryReader reader)
    {
      //UniqueId = new Guid(reader.ReadBytes(16));
      SequenceId = reader.ReadInt64();
      
      //var dataLength = reader.ReadInt32();
      //Data = reader.ReadBytes(dataLength);
    }
  }
}