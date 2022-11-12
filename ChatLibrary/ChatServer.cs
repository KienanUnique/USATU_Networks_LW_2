using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSimpleTcp;

namespace ChatLibrary
{
    public class ChatServer
    {
        public delegate void ClientConnectedHandler(ConnectionEventArgs e);

        public event ClientConnectedHandler ClientConnected;

        public delegate void ClientDisconnectedHandler(ConnectionEventArgs e);

        public event ClientDisconnectedHandler ClientDisconnected;

        public delegate void DataReceivedHandler(DecodedDataReceivedEventArgs e);

        public event DataReceivedHandler DataReceived;

        private readonly SimpleTcpServer _simpleTcpServer;
        private readonly string _nick;
        private readonly List<string> _onlineClientsList = new();

        public ChatServer(string ipPort, string nick)
        {
            _nick = nick;
            _simpleTcpServer = new SimpleTcpServer(ipPort);

            _simpleTcpServer.Events.ClientConnected += OnClientConnected;
            _simpleTcpServer.Events.ClientDisconnected += OnClientDisconnected;
            _simpleTcpServer.Events.DataReceived += OnDataFromClientReceived;
        }

        public void StartServer()
        {
            _simpleTcpServer.Start();
        }

        public void StopServer()
        {
            _simpleTcpServer.Stop();
            foreach (var client in _onlineClientsList)
            {
                _simpleTcpServer.DisconnectClient(client);
            }
            _simpleTcpServer.Events.ClientConnected -= OnClientConnected;
            _simpleTcpServer.Events.ClientDisconnected -= OnClientDisconnected;
            _simpleTcpServer.Events.DataReceived -= OnDataFromClientReceived;
        }
        
        
        private void SendMessageToClient(string client, string message)
        {
            _simpleTcpServer.Send(client, NetworkTools.GetStringJsonSendMessage(new MessageRequest(_nick, message)));
        }
        
        private void SendClientMessageToAnotherClient(string client, string message, string nickFromClient)
        {
            _simpleTcpServer.Send(client, NetworkTools.GetStringJsonSendMessage(new MessageRequest(nickFromClient, message)));
        }
        
        public void SendMessageToAllClients(string message)
        {
            foreach (var client in _onlineClientsList)
            {
                SendMessageToClient(client, message);
            }
        }

        private void SendClientMessageToAllOtherClients(string message, string fromClientIpPort, string fromClientNick)
        {
            foreach (var client in _onlineClientsList.Where(client => client != fromClientIpPort))
            {
                SendClientMessageToAnotherClient(client, message, fromClientNick);
            }
        }

        private void OnClientConnected(object sender, ConnectionEventArgs e)
        {
            _onlineClientsList.Add(e.IpPort);
            ClientConnected?.Invoke(e);
        }

        private void OnClientDisconnected(object sender, ConnectionEventArgs e)
        {
            _onlineClientsList.Remove(e.IpPort);
            ClientDisconnected?.Invoke(e);
        }

        private void OnDataFromClientReceived(object sender, DataReceivedEventArgs e)
        {
            var decodedData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            var messageRequest = NetworkTools.GetMessageRequestFromJson(decodedData);
            SendClientMessageToAllOtherClients(messageRequest.Message, e.IpPort, messageRequest.Nick);
            DataReceived?.Invoke(
                new DecodedDataReceivedEventArgs(e.IpPort, decodedData));
        }
    }
}