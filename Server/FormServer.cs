using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ChatLibrary;
using SuperSimpleTcp;

// TODO: add writeClientsData
namespace Server
{
    public partial class FormServer : Form
    {
        private ChatServer _server;
        private bool _isStarted = false;

        private const string ButtonStartText = "Start";
        private const string ButtonStopText = "Stop";

        public FormServer()
        {
            InitializeComponent();
        }

        private void Log(string logText, Color selectedColor)
        {
            richTextBoxChat.SelectionColor = selectedColor;
            var messageStringBuilder = new StringBuilder();
            messageStringBuilder.Append(DateTime.Now.ToString("HH:mm:ss"));
            messageStringBuilder.Append(": ");
            messageStringBuilder.Append(logText);
            messageStringBuilder.Append(Environment.NewLine);

            richTextBoxChat.AppendText(messageStringBuilder.ToString());
        }

        private void OnLog(string logMessage)
        {
            Log(logMessage, Color.Black);
        }

        private void OnClientConnected(ConnectionEventArgs e)
        {
            Log($"[{e.IpPort}] client connected", LogsColors.SystemConnected);
        }

        private void OnClientDisconnected(UserEventArgs e)
        {
            Log($"[{e.IpPort}] [{e.Nick}]: disconnected", LogsColors.SystemDisconnected);
        }

        private void OnClientAuthorize(UserEventArgs e)
        {
            Log($"[{e.IpPort}] client authorized with nick: {e.Nick}", LogsColors.SystemConnected);
        }

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            Log($"[{e.IpPort}] [{e.Nick}]: {e.Message}", LogsColors.Message);
        }

        private void richTextBoxChat_TextChanged(object sender, EventArgs e)
        {
            richTextBoxChat.SelectionStart = richTextBoxChat.Text.Length;
            richTextBoxChat.ScrollToCaret();
        }

        private void SendMessageAndEmptyTextBoxMessage()
        {
            if (textBoxMessageInput.Text == string.Empty) return;
            _server.SendMessageToAllClients(textBoxMessageInput.Text);
            Log($"[Me] {textBoxMessageInput.Text}", LogsColors.SelfMessage);
            textBoxMessageInput.Text = string.Empty;
        }

        private void buttonServerStatusChange_Click(object sender, EventArgs e)
        {
            if (!_isStarted && NetworkTools.IsAddressAndPortCorrect(textBoxIP.Text, textBoxPort.Text) &&
                textBoxNick.Text != string.Empty)
            {
                _server = new ChatServer(textBoxIP.Text + ":" + textBoxPort.Text, textBoxNick.Text);
                _server.LogThis += OnLog;
                _server.ClientConnected += OnClientConnected;
                _server.ClientDisconnected += OnClientDisconnected;
                _server.MessageReceived += OnMessageReceived;
                _server.ClientAuthorize += OnClientAuthorize;
                _server.StartServer();
                _isStarted = true;
                panelConnection.Enabled = false;
                panelChat.Enabled = true;
                buttonServerStatusChange.Text = ButtonStopText;
            }
            else if (_isStarted)
            {
                _server.StopServer();
                _server.LogThis -= OnLog;
                _server.ClientConnected -= OnClientConnected;
                _server.ClientDisconnected -= OnClientDisconnected;
                _server.MessageReceived -= OnMessageReceived;
                _server.ClientAuthorize -= OnClientAuthorize;
                _isStarted = false;
                panelConnection.Enabled = true;
                panelChat.Enabled = false;
                buttonServerStatusChange.Text = ButtonStartText;
            }
        }

        private void textBoxMessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessageAndEmptyTextBoxMessage();
            }
        }

        private void buttonSendMessage_MouseClick(object sender, MouseEventArgs e)
        {
            SendMessageAndEmptyTextBoxMessage();
        }
    }
}