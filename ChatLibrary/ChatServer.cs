using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSimpleTcp;

namespace ChatLibrary
{
    public class ChatServer
    {
        public delegate void LogHandler(string e);

        public event LogHandler LogThis;

        public delegate void ClientConnectedHandler(ConnectionEventArgs e);

        public event ClientConnectedHandler ClientConnected;

        public delegate void ClientAuthorizedHandler(AuthorizeEventArgs e);

        public event ClientAuthorizedHandler ClientAuthorize;

        public delegate void ClientDisconnectedHandler(ConnectionEventArgs e);

        public event ClientDisconnectedHandler ClientDisconnected;

        public delegate void MessageReceivedHandler(MessageReceivedEventArgs e);

        public event MessageReceivedHandler MessageReceived;

        private struct Client
        {
            public string Nick { get; }
            public string IpPort { get; }
        }

        private readonly SimpleTcpServer _simpleTcpServer;
        private readonly SenderInfo _thisSenderInfo;
        private readonly List<SenderInfo> _onlineClientsList = new();

        public ChatServer(string ipPort, string nick)
        {
            _thisSenderInfo = new SenderInfo(nick, ipPort);
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
                _simpleTcpServer.DisconnectClient(client.IpPort);
            }

            _simpleTcpServer.Events.ClientConnected -= OnClientConnected;
            _simpleTcpServer.Events.ClientDisconnected -= OnClientDisconnected;
            _simpleTcpServer.Events.DataReceived -= OnDataFromClientReceived;
        }

        private void SendPocketToAllClients(PocketTCP pocketTcp)
        {
            string pocketTcpString = NetworkTools.GetStringJsonSendMessage(pocketTcp);
            foreach (var client in _onlineClientsList)
            {
                _simpleTcpServer.Send(client.IpPort, pocketTcpString);
            }
        }

        private void SendClientPocketToAnotherClients(PocketTCP pocketTcp)
        {
            string pocketTcpString = NetworkTools.GetStringJsonSendMessage(pocketTcp);
            foreach (var client in _onlineClientsList.Where(client => client.IpPort != pocketTcp.SenderIpPort))
            {
                _simpleTcpServer.Send(client.IpPort, pocketTcpString);
            }
        }

        private void SendClientAuthorizeToAnotherClients(SenderInfo connectedClient)
        {
            var pocket = new PocketTCP(connectedClient.Nick, connectedClient.IpPort, RequestsTypes.Authorize);
            SendClientPocketToAnotherClients(pocket);
        }

        private void SendClientDisconnectionToAnotherClient(SenderInfo disconnectedClient)
        {
            var pocket = new PocketTCP(disconnectedClient.Nick, disconnectedClient.IpPort, RequestsTypes.Disconnection);
            SendClientPocketToAnotherClients(pocket);
        }

        public void SendMessageToAllClients(string message)
        {
            var pocket = new PocketTCP(_thisSenderInfo.Nick, _thisSenderInfo.IpPort, RequestsTypes.Message, message);
            SendPocketToAllClients(pocket);
        }

        private void ProcessReceivedPocket(PocketTCP receivedPocket)
        {
            var senderInfo = new SenderInfo(receivedPocket.SenderNick, receivedPocket.SenderIpPort);
            switch (receivedPocket.RequestType)
            {
                case RequestsTypes.Authorize:
                    _onlineClientsList.Add(senderInfo);
                    SendClientAuthorizeToAnotherClients(senderInfo);
                    ClientAuthorize?.Invoke(new AuthorizeEventArgs(senderInfo.IpPort, senderInfo.Nick));
                    break;

                case RequestsTypes.Disconnection:
                    if (_onlineClientsList.Contains(senderInfo))
                    {
                        try
                        {
                            _simpleTcpServer.DisconnectClient(senderInfo.IpPort);
                        }
                        catch
                        {
                            // ignored
                        }

                        _onlineClientsList.Remove(senderInfo);
                        SendClientDisconnectionToAnotherClient(senderInfo);
                        ClientDisconnected?.Invoke(new ConnectionEventArgs(senderInfo.IpPort, DisconnectReason.Normal));
                    }

                    break;

                case RequestsTypes.Message:
                    if (_onlineClientsList.Contains(senderInfo))
                    {
                        SendClientPocketToAnotherClients(receivedPocket);
                        MessageReceived?.Invoke(new MessageReceivedEventArgs(senderInfo.IpPort,
                            senderInfo.Nick, receivedPocket.Message));
                    }

                    break;
            }
        }

        private void OnClientConnected(object sender, ConnectionEventArgs e)
        {
            ClientConnected?.Invoke(e);
        }

        private void OnClientDisconnected(object sender, ConnectionEventArgs e)
        {
            if (_onlineClientsList.Exists(i => i.IpPort == e.IpPort))
            {
                _onlineClientsList.Remove(_onlineClientsList.Find(i => i.IpPort == e.IpPort));
            }

            ClientDisconnected?.Invoke(e);
        }

        private void OnDataFromClientReceived(object sender, DataReceivedEventArgs e)
        {
            var decodedData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            var pocketTcp = NetworkTools.GetPocketTcpFromJson(decodedData);
            ProcessReceivedPocket(pocketTcp);
        }
    }
}