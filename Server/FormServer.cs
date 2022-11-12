using System;
using System.Drawing;
using System.Windows.Forms;
using ChatLibrary;
using SuperSimpleTcp;

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
            richTextBoxChat.AppendText(Environment.NewLine + logText);
        }

        private void OnClientConnected(ConnectionEventArgs e)
        {
            Log($"[{e.IpPort}] client connected", LogsColors.SystemConnected);
        }

        private void OnClientDisconnected(ConnectionEventArgs e)
        {
            Log($"[{e.IpPort}] client disconnected", LogsColors.SystemDisconnected);
        }

        private void OnDataReceived(DecodedDataReceivedEventArgs e)
        {
            var messageRequest = NetworkTools.GetMessageRequestFromJson(e.Data);
            Log($"[{e.IpPort}][{messageRequest.Nick}]: {messageRequest.Message}", LogsColors.Message);
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
                _server.ClientConnected += OnClientConnected;
                _server.ClientDisconnected += OnClientDisconnected;
                _server.DataReceived += OnDataReceived;
                _server.StartServer();
                _isStarted = true;
                panelConnection.Enabled = false;
                panelChat.Enabled = true;
                buttonServerStatusChange.Text = ButtonStopText;
            }
            else if (_isStarted)
            {
                _server.StopServer();
                _server.ClientConnected -= OnClientConnected;
                _server.ClientDisconnected -= OnClientDisconnected;
                _server.DataReceived -= OnDataReceived;
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