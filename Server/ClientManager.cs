using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
  public class ClientManager
  {
    private readonly ConnectionManager _connectionManager;
    private readonly Dictionary<int, Client> _clients = new Dictionary<int, Client>();

    private static int _clientCounter;

    public ClientManager(ConnectionManager connectionManager)
    {
      _connectionManager = connectionManager;
    }

    public void Start()
    {
      _connectionManager.ClientConnectedAsync += ClientConnectedAsync;
    }

    public void Stop()
    {
      _connectionManager.ClientConnectedAsync -= ClientConnectedAsync;
      foreach (var kvp in _clients) {
        var client = kvp.Value;
        client.Close();
      }
    }

    private void ClientConnectedAsync(Socket socket)
    {
      var clientId = Interlocked.Increment(ref _clientCounter);
      var client = new Client(clientId, socket);
      lock (_clients)
        _clients.Add(client.Id, client);

      Console.WriteLine("Client connected: " + client.RemoteAddr);
    }
  }
}