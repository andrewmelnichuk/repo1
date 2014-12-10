using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Client
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("connecting to server at tcp://localhost:12345 ...");

      var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      socket.Connect(IPAddress.Loopback, 12345);

      Console.WriteLine("connected to server");

      while (true) {
        Console.Write("client: ");
        var input = Console.ReadLine();
        var bytes = Encoding.UTF8.GetBytes(input ?? "");

        var request = new MessageBase(bytes);

        socket.Send(BitConverter.GetBytes(8));
        socket.Send(BitConverter.GetBytes(1L));
//        // write
//        MemoryStream stream;
//        using (stream = new MemoryStream())
//        using (var writer = new BinaryWriter(stream)) {
//          request.Write(writer);
//          stream.Seek(0, SeekOrigin.Begin);
//          socket.Send(BitConverter.GetBytes(stream.Length));
//          socket.Send(stream.ToArray());
//        }

        // read
//        var lenBuf = new byte[4];
//        socket.Receive(lenBuf);
//        var bytesRead = BitConverter.ToInt32(lenBuf, 0);
//
//        using (stream = new MemoryStream(bytesRead)) {
//          var readBuf = new byte[10];
//          while (bytesRead > 0) {
//            var read = socket.Receive(readBuf);
//            stream.Write(readBuf, 0, read);
//            bytesRead -= read;
//          }
//
//          var response = new MessageBase();
//          using (var reader = new BinaryReader(stream))
//            response.Read(reader);
//
//          Console.WriteLine("server: #{0} {1}", response.SequenceId, 
//            Encoding.UTF8.GetString(response.Data));
//        }
      }
    }
  }
}
