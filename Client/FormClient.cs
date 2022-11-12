using System;
using System.Drawing;
using System.Windows.Forms;
using ChatLibrary;
using SuperSimpleTcp;
using static System.String;

namespace Client
{
    public partial class FormClient : Form
    {
        private ChatClient _client;
        private bool _isConnected = false;

        private const string ButtonConnectText = "Connect";
        private const string ButtonDisconnectText = "Disconnect";

        public FormClient()
        {
            InitializeComponent();
        }

        private void Log(string logText, Color selectedColor)
        {
            richTextBoxChat.SelectionColor = selectedColor;
            richTextBoxChat.AppendText(Environment.NewLine + logText);
        }

        private void OnConnected(ConnectionEventArgs e)
        {
            Log($"*** Server {e.IpPort} connected", LogsColors.SystemConnected);
        }

        private void OnDisconnected(ConnectionEventArgs e)
        {
            Log($"*** Server {e.IpPort} disconnected", LogsColors.SystemDisconnected);
            _isConnected = false;
            SwitchToConnectionInterface();
        }

        private void OnDataReceived(DecodedDataReceivedEventArgs e)
        {
            var messageRequest = NetworkTools.GetMessageRequestFromJson(e.Data);
            Log($"[{e.IpPort}][{messageRequest.Nick}]: {messageRequest.Message}", LogsColors.Message);
        }

        private void SendMessageAndEmptyTextBoxMessage()
        {
            if (textBoxMessageInput.Text == Empty) return;
            _client.SendMessage(textBoxMessageInput.Text);
            Log($"[Me] {textBoxMessageInput.Text}", LogsColors.SelfMessage);
            textBoxMessageInput.Text = Empty;
        }

        private void buttonSendMessage_MouseClick(object sender, MouseEventArgs e)
        {
            SendMessageAndEmptyTextBoxMessage();
        }

        private void textBoxMessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessageAndEmptyTextBoxMessage();
            }
        }

        private void SwitchToChatInterface()
        {
            _client.Connected += OnConnected;
            _client.Disconnected += OnDisconnected;
            _client.DataReceived += OnDataReceived;
            panelConnection.Enabled = false;
            panelChat.Enabled = true;
            buttonConnectionStatusChange.Text = ButtonDisconnectText;
        }
        
        private void SwitchToConnectionInterface()
        {
            _client.Connected -= OnConnected;
            _client.Disconnected -= OnDisconnected;
            _client.DataReceived -= OnDataReceived;
            panelConnection.Enabled = true;
            panelChat.Enabled = false;
            buttonConnectionStatusChange.Text = ButtonConnectText;
        }

        private void buttonConnectionStatusChange_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isConnected && NetworkTools.IsAddressAndPortCorrect(textBoxIP.Text, textBoxPort.Text) &&
                textBoxNick.Text != string.Empty)
            {
                _client = new ChatClient(textBoxIP.Text + ":" + textBoxPort.Text, textBoxNick.Text);
                SwitchToChatInterface();
                _client.Connect();
                _isConnected = true;
            }
            else if (_isConnected)
            {
                _client.Disconnect();
                SwitchToConnectionInterface();
                _isConnected = false;
            }
        }

        private void richTextBoxChat_TextChanged(object sender, EventArgs e)
        {
            richTextBoxChat.SelectionStart = richTextBoxChat.Text.Length;
            richTextBoxChat.ScrollToCaret();
        }
    }
}