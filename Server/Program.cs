using System;
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
          var t = new Thread(HandleClient);
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
        while (true)
        {
          if (!socket.Connected) break;

          // read
          var readBuf = new byte[4];
          var r = socket.Receive(readBuf);
          var bytesRead = BitConverter.ToInt32(readBuf, 0);
          var readBuf2 = new byte[8];
          var r2 = socket.Receive(readBuf2);
          Console.WriteLine(BitConverter.ToInt64(readBuf2, 0));
//          var request = new MessageBase();
//          MemoryStream stream;
          /*
          using (stream = new MemoryStream(bytesRead)) {
            while (bytesRead > 0) {
              var read = socket.Receive(readBuf);
              stream.Write(readBuf, 0, read);
              bytesRead -= read;
            }

            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(stream))
              request.Read(reader);

            Console.WriteLine("client: #{0} {1}", request.SequenceId,
              Encoding.UTF8.GetString(request.Data ?? new byte[0]));
          }
          */
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
