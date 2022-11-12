using System.Text;
using SuperSimpleTcp;

namespace ChatLibrary
{
    public class ChatClient
    {
        public delegate void ConnectedHandler(ConnectionEventArgs e);

        public event ConnectedHandler Connected;

        public delegate void DisconnectedHandler(ConnectionEventArgs e);

        public event DisconnectedHandler Disconnected;

        public delegate void DataReceivedHandler(DecodedDataReceivedEventArgs e);

        public event DataReceivedHandler DataReceived;


        private const int ConnectionTimeoutMs = 10000;
        private readonly SimpleTcpClient _simpleTcpClient;

        private readonly string _nick;

        public ChatClient(string ipPort, string nick)
        {
            _nick = nick;
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
            _simpleTcpClient.Disconnect();
            _simpleTcpClient.Events.Connected -= OnConnected;
            _simpleTcpClient.Events.Disconnected -= OnDisconnected;
            _simpleTcpClient.Events.DataReceived -= OnDataReceived;
        }

        public void SendMessage(string message)
        {
            _simpleTcpClient.Send(NetworkTools.GetStringJsonSendMessage(new MessageRequest(_nick, message)));
        }

        private void OnConnected(object sender, ConnectionEventArgs e)
        {
            Connected?.Invoke(e);
        }

        private void OnDisconnected(object sender, ConnectionEventArgs e)
        {
            Disconnected?.Invoke(e);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(
                new DecodedDataReceivedEventArgs(e.IpPort, Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count)));
        }
    }
}