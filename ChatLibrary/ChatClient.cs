using System;
using System.Text;
using SuperSimpleTcp;

namespace ChatLibrary
{
    public class ChatClient // TODO: the same interface for ChatClient and ChatServer?  
    {
        public delegate void LogHandler(string e);

        public event LogHandler LogThis;

        public delegate void ReadyToUseHandler(ConnectionEventArgs e);

        public event ReadyToUseHandler ReadyToUse;

        public delegate void
            AuthenticationErrorHandler(AuthenticationErrorEventArgs e); // TODO: make custom arguments for handler

        public event AuthenticationErrorHandler AuthenticationError;

        public delegate void DisconnectedHandler(ConnectionEventArgs e);

        public event DisconnectedHandler Disconnected;

        public delegate void ServerIsUnreachableHandler(ConnectionEventArgs e);

        public event ServerIsUnreachableHandler ServerIsUnreachable;

        public delegate void MessageReceivedHandler(MessageReceivedEventArgs e);

        public event MessageReceivedHandler MessageReceived;

        public delegate void AnotherClientAuthorizedHandler(UserEventArgs e);

        public event AnotherClientAuthorizedHandler AnotherClientAuthorize;

        public delegate void AnotherClientDisconnectedHandler(UserEventArgs e);

        public event AnotherClientDisconnectedHandler AnotherClientDisconnected;


        private const int ConnectionTimeoutMs = 10000;

        private SimpleTcpClient _simpleTcpClient;
        private string _localNick;
        private string _localPassword;
        private bool _needSignUp;

        public void TryAuthorize(string ipPort, string nick, string localPassword, bool needSignUp)
        {
            _localNick = nick;
            _localPassword = localPassword;
            _needSignUp = needSignUp;
            _simpleTcpClient = new SimpleTcpClient(ipPort);

            _simpleTcpClient.Events.Connected += OnConnected;
            _simpleTcpClient.Events.Disconnected += OnDisconnected;
            _simpleTcpClient.Events.DataReceived += OnDataReceived;
            Connect();
        }

        public void Disconnect()
        {
            SendDisconnectionRequest();
            _simpleTcpClient.Disconnect();
            _simpleTcpClient.Events.Connected -= OnConnected;
            _simpleTcpClient.Events.Disconnected -= OnDisconnected;
            _simpleTcpClient.Events.DataReceived -= OnDataReceived;
        }
        
        private void Connect()
        {
            try
            {
                _simpleTcpClient.ConnectWithRetries(ConnectionTimeoutMs);
            }
            catch (Exception)
            {
                ServerIsUnreachable?.Invoke(new ConnectionEventArgs(_simpleTcpClient.ServerIpPort));
            }
        }

        private async void SendPocket(PocketTCP pocketTcp)
        {
            await _simpleTcpClient.SendAsync(NetworkTools.GetStringJsonSendMessage(pocketTcp));
        }

        private void SendLogInRequest()
        {
            var pocketTcp = new PocketTCP(_localNick, _simpleTcpClient.LocalEndpoint.ToString(),
                RequestsTypes.LogIn, _localPassword);
            SendPocket(pocketTcp);
        }

        private void SendSignUpRequest()
        {
            var pocketTcp = new PocketTCP(_localNick, _simpleTcpClient.LocalEndpoint.ToString(),
                RequestsTypes.SignUp, _localPassword);
            SendPocket(pocketTcp);
        }

        private void SendDisconnectionRequest()
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
                case RequestsTypes.NewUserAuthorize:
                    AnotherClientAuthorize?.Invoke(new UserEventArgs(receivedPocket.SenderIpPort,
                        receivedPocket.SenderNick));
                    break;
                case RequestsTypes.AnswerOnAuthentication:
                    Enum.TryParse(receivedPocket.Message, out AnswerOnAuthenticationTypes answerOnAuthentication);
                    switch (answerOnAuthentication)
                    {
                        case AnswerOnAuthenticationTypes.Ok:
                            ReadyToUse?.Invoke(new ConnectionEventArgs(receivedPocket.SenderIpPort));
                            break;
                        default:
                            AuthenticationError?.Invoke(new AuthenticationErrorEventArgs(receivedPocket.SenderIpPort,
                                answerOnAuthentication));
                            break;
                    }

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
            if (_needSignUp)
            {
                SendSignUpRequest();
            }
            else
            {
                SendLogInRequest();
            }
        }

        private void OnDisconnected(object sender, ConnectionEventArgs e)
        {
            Disconnected?.Invoke(e);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            var decodedData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            LogThis?.Invoke(decodedData);
            var pocketTcp = NetworkTools.GetPocketTcpFromJson(decodedData);
            ProcessReceivedPocket(pocketTcp);
        }
    }
}