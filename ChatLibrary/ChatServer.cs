﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSimpleTcp;
using DataReceivedEventArgs = SuperSimpleTcp.DataReceivedEventArgs;

namespace ChatLibrary
{
    public class ChatServer
    {
        public delegate void LogHandler(string e);

        public event LogHandler LogThis;

        public delegate void ClientConnectedHandler(ConnectionEventArgs e);

        public event ClientConnectedHandler ClientConnected;

        public delegate void ClientAuthorizedHandler(UserEventArgs e);

        public event ClientAuthorizedHandler ClientAuthorize;

        public delegate void ClientDisconnectedHandler(UserEventArgs e);

        public event ClientDisconnectedHandler ClientDisconnected;

        public delegate void MessageReceivedHandler(MessageReceivedEventArgs e);

        public event MessageReceivedHandler MessageReceived;

        private readonly SimpleTcpServer _simpleTcpServer;
        private readonly SenderInfo _thisSenderInfo;
        private readonly List<SenderInfo> _loggedInClientsIpList = new();
        private readonly ClientsDataBaseWithFileStorage _clientsDataBaseWithFileStorage = new();

        public ChatServer(string ipPort, string nick)
        {
            _thisSenderInfo = new SenderInfo(nick, ipPort);
            _simpleTcpServer = new SimpleTcpServer(ipPort);

            _simpleTcpServer.Events.ClientConnected += OnClientConnected;
            _simpleTcpServer.Events.ClientDisconnected += OnClientDisconnected;
            _simpleTcpServer.Events.DataReceived += OnDataFromClientReceived;

            _clientsDataBaseWithFileStorage.ReadClientsData();
        }

        public void StartServer()
        {
            _simpleTcpServer.Start();
        }

        public void StopServer()
        {
            _simpleTcpServer.Stop();
            foreach (var client in _loggedInClientsIpList)
            {
                _simpleTcpServer.DisconnectClient(client.IpPort);
            }

            _simpleTcpServer.Events.ClientConnected -= OnClientConnected;
            _simpleTcpServer.Events.ClientDisconnected -= OnClientDisconnected;
            _simpleTcpServer.Events.DataReceived -= OnDataFromClientReceived;
        }

        private void SendPocket(string ipPort, PocketTCP pocketTcp)
        {
            string pocketTcpString = NetworkTools.GetStringJsonSendMessage(pocketTcp);
            SendPocket(ipPort, pocketTcpString);
        }

        private async void SendPocket(string ipPort, string pocketTcp)
        {
            LogThis?.Invoke(pocketTcp);
            await _simpleTcpServer.SendAsync(ipPort, pocketTcp);
        }

        private void SendPocketToAllClients(PocketTCP pocketTcp)
        {
            string pocketTcpString = NetworkTools.GetStringJsonSendMessage(pocketTcp);
            foreach (var client in _loggedInClientsIpList)
            {
                SendPocket(client.IpPort, pocketTcpString);
            }
        }

        private void SendClientPocketToAnotherClients(PocketTCP pocketTcp)
        {
            string pocketTcpString = NetworkTools.GetStringJsonSendMessage(pocketTcp);
            foreach (var client in _loggedInClientsIpList.Where(client => client.IpPort != pocketTcp.SenderIpPort))
            {
                SendPocket(client.IpPort, pocketTcpString);
            }
        }

        private void SendClientAuthorizeToAnotherClients(SenderInfo connectedClient)
        {
            var pocket = new PocketTCP(connectedClient.Nick, connectedClient.IpPort, RequestsTypes.NewUserAuthorize);
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
            switch (receivedPocket.RequestType) // TODO: Make requests classes with polymorphism and requests factory 
            {
                case RequestsTypes.SignUp:
                    ProcessUserSignUp(receivedPocket);
                    break;

                case RequestsTypes.LogIn:
                    ProcessUserLogIn(receivedPocket);
                    break;

                case RequestsTypes.Disconnection:
                    ProcessUserDisconnection(receivedPocket);
                    break;

                case RequestsTypes.Message:
                    ProcessUserMessage(receivedPocket);
                    break;
            }
        }

        private void OnClientConnected(object sender, ConnectionEventArgs e)
        {
            ClientConnected?.Invoke(e);
        }

        private void OnClientDisconnected(object sender, ConnectionEventArgs e)
        {
            if (_loggedInClientsIpList.Exists(i => i.IpPort == e.IpPort))
            {
                var foundedClient = _loggedInClientsIpList.Find(i => i.IpPort == e.IpPort);
                _loggedInClientsIpList.Remove(foundedClient);
                ClientDisconnected?.Invoke(new UserEventArgs(foundedClient.IpPort, foundedClient.Nick));
            }
        }

        private void OnDataFromClientReceived(object sender, DataReceivedEventArgs e)
        {
            var decodedData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            LogThis?.Invoke(decodedData);
            var pocketTcp = NetworkTools.GetPocketTcpFromJson(decodedData);
            ProcessReceivedPocket(pocketTcp);
        }

        private void ProcessUserLogIn(PocketTCP receivedPocket)
        {
            bool isLogInSuccess = _clientsDataBaseWithFileStorage.IsClientsPasswordCorrect(receivedPocket.SenderNick,
                receivedPocket.Message);
            if (isLogInSuccess)
            {
                var loggedInUser = new SenderInfo(receivedPocket.SenderNick, receivedPocket.SenderIpPort);
                _loggedInClientsIpList.Add(loggedInUser);
                SendClientAuthorizeToAnotherClients(loggedInUser);
                ClientAuthorize?.Invoke(new UserEventArgs(loggedInUser.IpPort, loggedInUser.Nick));
            }

            SendPocket(receivedPocket.SenderIpPort,
                PrepareAnswerOnAuthenticationRequest(isLogInSuccess));
        }

        private void ProcessUserSignUp(PocketTCP receivedPocket)
        {
            bool addResult =
                _clientsDataBaseWithFileStorage.TryAddUser(receivedPocket.SenderNick, receivedPocket.Message);
            if (addResult)
            {
                var signedUpUser = new SenderInfo(receivedPocket.SenderNick, receivedPocket.SenderIpPort);
                _loggedInClientsIpList.Add(signedUpUser);
                SendClientAuthorizeToAnotherClients(signedUpUser);
                ClientAuthorize?.Invoke(new UserEventArgs(signedUpUser.IpPort, signedUpUser.Nick));
            }

            SendPocket(receivedPocket.SenderIpPort,
                PrepareAnswerOnAuthenticationRequest(addResult));
        }

        private PocketTCP PrepareAnswerOnAuthenticationRequest(bool isOk)
        {
            return new PocketTCP(_thisSenderInfo.Nick, _thisSenderInfo.IpPort, RequestsTypes.AnswerOnAuthentication,
                (isOk ? AnswerOnAuthenticationTypes.Ok : AnswerOnAuthenticationTypes.NotOk).ToString());
        }

        private void ProcessUserDisconnection(PocketTCP disconnectedUserInfo)
        {
            var disconnectedUser = new SenderInfo(disconnectedUserInfo.SenderNick, disconnectedUserInfo.SenderIpPort);
            if (_loggedInClientsIpList.Contains(disconnectedUser))
            {
                try
                {
                    _simpleTcpServer.DisconnectClient(disconnectedUserInfo.SenderIpPort);
                }
                catch
                {
                    // ignored
                }

                _loggedInClientsIpList.Remove(disconnectedUser);
                SendClientDisconnectionToAnotherClient(disconnectedUser);
                ClientDisconnected?.Invoke(new UserEventArgs(disconnectedUser.IpPort, disconnectedUser.Nick));
            }
        }

        private void ProcessUserMessage(PocketTCP receivedPocket)
        {
            if (_loggedInClientsIpList.Exists(user =>
                    user.Nick == receivedPocket.SenderNick && user.IpPort == receivedPocket.SenderIpPort))
            {
                SendClientPocketToAnotherClients(receivedPocket);
                MessageReceived?.Invoke(new MessageReceivedEventArgs(receivedPocket.SenderIpPort,
                    receivedPocket.SenderNick, receivedPocket.Message));
            }
        }
    }
}