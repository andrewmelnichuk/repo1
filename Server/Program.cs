using System;

namespace Server
{
  class Program
  {
    static void Main(string[] args)
    {
      var port = 12345;
      if (args.Length == 1)
        Int32.TryParse(args[0], out port);

      var service = new Service(port);
      service.Start();

      Console.WriteLine("Press ENTER to stop");
      if (Console.Read() == (int) ConsoleKey.Enter)
        service.Stop();
    }

/*
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
    */
 }
}
