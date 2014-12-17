using System;
using System.Net;
using System.Net.Sockets;
//using System.Threading;

namespace Server
{
  public delegate void ClientConnectedDelegate(Socket socket);

  public class ConnectionManager
  {
    private readonly int _port;
    private Socket _socket;
    private bool _closing;
//    private Thread _thread;

    public ConnectionManager(int port)
    {
      _port = port;
    }

    public void Start()
    {
      OpenSocket();
      AcceptConnections();
    }

    private void AcceptConnections()
    {
      _socket.BeginAccept(AcceptCallback, null);
    }

    private void AcceptCallback(IAsyncResult ar)
    {
      // callback also called after socket closed
      if (_closing)
        return;

      var clientSocket = _socket.EndAccept(ar);
      AcceptConnections();

      OnClientConnected(clientSocket);
    }

    private void OnClientConnected(Socket socket)
    {
      if (ClientConnectedAsync != null)
        ClientConnectedAsync(socket);
    }

    /*
    private void ThreadProc()
    {
      while (true)
      {
        try
        {
          var clientSocket = _socket.Accept();
          var t = new Thread(HandleClient) { IsBackground = true };
          t.Start(clientSocket);
        }
        catch (Exception ex)
        {

          if (_shutdown)
          {
            Console.WriteLine("shutting down...");
            break;
          }

          Console.WriteLine(ex.ToString());

          try
          {
            Console.WriteLine("reopen socket");
            CloseSocket();
            OpenSocket();
          }
          catch (Exception ex2)
          {
            Console.WriteLine("unable to reopen socket");
            Console.WriteLine(ex2.ToString());
            break;
          }
        }
      }
    }
    */
    public void Stop()
    {
      _closing = true;
      CloseSocket();
    }

    public event ClientConnectedDelegate ClientConnectedAsync;

    private void OpenSocket()
    {
      _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
      var endPoind = new IPEndPoint(IPAddress.Any, _port);

      _socket.Bind(endPoind);
      _socket.Listen(10000);
    }

    private void CloseSocket()
    {
      _socket.Close();
    }
  }
}