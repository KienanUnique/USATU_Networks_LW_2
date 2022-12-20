using System.Text;
using SuperSimpleTcp;

namespace ChatLibrary
{
    public class ChatClient
    {
        public delegate void LogHandler(string e);

        public event LogHandler LogThis;

        public delegate void ConnectedHandler(ConnectionEventArgs e);

        public event ConnectedHandler Connected;

        public delegate void DisconnectedHandler(ConnectionEventArgs e);

        public event DisconnectedHandler Disconnected;

        public delegate void MessageReceivedHandler(MessageReceivedEventArgs e);

        public event MessageReceivedHandler MessageReceived;

        public delegate void AnotherClientAuthorizedHandler(UserEventArgs e);

        public event AnotherClientAuthorizedHandler AnotherClientAuthorize;

        public delegate void AnotherClientDisconnectedHandler(UserEventArgs e);

        public event AnotherClientDisconnectedHandler AnotherClientDisconnected;


        private const int ConnectionTimeoutMs = 10000;

        private readonly SimpleTcpClient _simpleTcpClient;
        private readonly string _localNick;

        public ChatClient(string ipPort, string nick)
        {
            _localNick = nick;
            _simpleTcpClient = new SimpleTcpClient(ipPort);

            _simpleTcpClient.Events.Connected += OnConnected;
            _simpleTcpClient.Events.Disconnected += OnDisconnected;
            _simpleTcpClient.Events.DataReceived += OnDataReceived;
        }

        public void Connect()
        {
            _simpleTcpClient.ConnectWithRetries(ConnectionTimeoutMs);
        }

        public void Disconnect()
        {
            SendDisconnectionRequest();
            _simpleTcpClient.Disconnect();
            _simpleTcpClient.Events.Connected -= OnConnected;
            _simpleTcpClient.Events.Disconnected -= OnDisconnected;
            _simpleTcpClient.Events.DataReceived -= OnDataReceived;
        }

        private async void SendPocket(PocketTCP pocketTcp)
        {
            await _simpleTcpClient.SendAsync(NetworkTools.GetStringJsonSendMessage(pocketTcp));
        }

        public void SendAuthorizeRequest()
        {
            var pocketTcp = new PocketTCP(_localNick, _simpleTcpClient.LocalEndpoint.ToString(),
                RequestsTypes.Authorize);
            SendPocket(pocketTcp);
        }

        public void SendDisconnectionRequest()
        {
            var pocketTcp = new PocketTCP(_localNick, _simpleTcpClient.LocalEndpoint.ToString(),
                RequestsTypes.Disconnection);
            SendPocket(pocketTcp);
        }

        public void SendMessage(string message)
        {
            var pocketTcp = new PocketTCP(_localNick, _simpleTcpClient.LocalEndpoint.ToString(), RequestsTypes.Message,
                message);
            SendPocket(pocketTcp);
        }

        private void ProcessReceivedPocket(PocketTCP receivedPocket)
        {
            switch (receivedPocket.RequestType)
            {
                case RequestsTypes.Authorize:
                    AnotherClientAuthorize?.Invoke(new UserEventArgs(receivedPocket.SenderIpPort,
                        receivedPocket.SenderNick));
                    break;

                case RequestsTypes.Disconnection:
                    AnotherClientDisconnected?.Invoke(new UserEventArgs(receivedPocket.SenderIpPort,
                        receivedPocket.SenderNick));
                    break;

                case RequestsTypes.Message:
                    MessageReceived?.Invoke(new MessageReceivedEventArgs(receivedPocket.SenderIpPort,
                        receivedPocket.SenderNick, receivedPocket.Message));
                    break;
            }
        }

        private void OnConnected(object sender, ConnectionEventArgs e)
        {
            SendAuthorizeRequest();
            Connected?.Invoke(e);
        }

        private void OnDisconnected(object sender, ConnectionEventArgs e)
        {
            Disconnected?.Invoke(e);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            var decodedData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            var pocketTcp = NetworkTools.GetPocketTcpFromJson(decodedData);
            ProcessReceivedPocket(pocketTcp);
        }
    }
}