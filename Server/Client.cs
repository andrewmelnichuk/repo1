using System.Net;
using System.Net.Sockets;

namespace Server
{
  public class Client
  {
    private readonly Socket _socket;

    public Client(int id, Socket socket)
    {
      _socket = socket;
      Id = id;
      SetRemoteAddr();
    }

    private void SetRemoteAddr()
    {
      var remoteEndPoint = (IPEndPoint) _socket.RemoteEndPoint;
      RemoteAddr = string.Format("{0}:{1}", remoteEndPoint.Address.ToString().Substring(7), remoteEndPoint.Port);
    }

    public int Id { get; private set; }

    public string RemoteAddr { get; private set; }

    public void Close()
    {
      _socket.Send(new byte[] {1, 2});
      _socket.Close();
    }
  }
}