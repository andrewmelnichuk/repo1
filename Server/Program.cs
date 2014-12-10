using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;

namespace Server
{
  class Program
  {
    private static Socket _socket;
    private static bool _shutdown;

    static void Main(string[] args)
    {
      OpenSocket();

      var t = new Thread(AcceptSocketProc) {IsBackground = true};
      t.Start();

      Console.WriteLine("server started on port " + ((IPEndPoint)_socket.LocalEndPoint).Port);
      Console.WriteLine("press ENTER to stop");
      Console.ReadLine();

      _shutdown = true;
      CloseSocket();
      t.Join();

      Console.WriteLine("server stopped");
    }

    private static void OpenSocket()
    {
      _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

      var endPoind = new IPEndPoint(IPAddress.Any, 12345);
      
      _socket.Bind(endPoind);
      _socket.Listen(10000);
    }

    private static void CloseSocket()
    {
      _socket.Close();
    }

    private static void AcceptSocketProc()
    {
      while (true)
      {
        try {
          var clientSocket = _socket.Accept();
          var t = new Thread(HandleClient) {IsBackground = true};
          t.Start(clientSocket);
        }
        catch (Exception ex) {

          if (_shutdown) {
            Console.WriteLine("shutting down...");
            break;
          }

          Console.WriteLine(ex.ToString());

          try {
            Console.WriteLine("reopen socket");
            CloseSocket();
            OpenSocket();
          }
          catch (Exception ex2) {
            Console.WriteLine("unable to reopen socket");
            Console.WriteLine(ex2.ToString());
            break;
          }
        }
      }
    }

    private static void HandleClient(object data)
    {
      var socket = (Socket) data;
      var remEndPoint = (IPEndPoint) socket.RemoteEndPoint;
      Console.WriteLine("client connected {0}:{1}", remEndPoint.Address, remEndPoint.Port);

      try {
        while (socket.Connected)
        {
          // read
          var lenBuf = new byte[4];
          var r = socket.Receive(lenBuf);
          var lenBytes = BitConverter.ToInt32(lenBuf, 0);

          var stream = new MemoryStream();
          while (lenBytes > 0) {
            var dataBuf = new byte[lenBytes];
            var readBytes = socket.Receive(dataBuf);
            stream.Write(dataBuf, 0, readBytes);
            lenBytes -= readBytes;
          }

          var message = new MessageBase();
          stream.Seek(0, SeekOrigin.Begin);
          using (var reader = new BinaryReader(stream))
            message.Read(reader);

          Console.WriteLine("message received:");
          Console.WriteLine("uid: " + message.UniqueId);
          Console.WriteLine("seq id: " + message.SequenceId);
          Console.WriteLine("data: " + Encoding.UTF8.GetString(message.Data));
          Console.WriteLine();

          // write
//          request.SequenceId++;
//
//          using (stream = new MemoryStream())
//          using (var writer = new BinaryWriter(stream)) {
//            request.Write(writer);
//            socket.Send(BitConverter.GetBytes(stream.Length));
//            socket.Send(stream.ToArray());
//          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
  }
}
