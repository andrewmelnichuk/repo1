using System;

namespace Server
{
  public class Service
  {
    private readonly int _port;
    private readonly ConnectionManager _connectionManager;
    private readonly ClientManager _clientManager;

    public Service(int port)
    {
      _port = port;
      _connectionManager = new ConnectionManager(port);
      _clientManager = new ClientManager(_connectionManager);
    }

    public void Start()
    {
      _connectionManager.Start();
      _clientManager.Start();

      Console.WriteLine("Server started on port " + _port);
    }

    public void Stop()
    {
      _connectionManager.Stop();
      _clientManager.Stop();

      Console.WriteLine("Server stopped");
    }
  }
}