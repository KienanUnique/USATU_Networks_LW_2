using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ChatLibrary;
using SuperSimpleTcp;
using static System.String;

namespace Client
{
    public partial class FormClient : Form
    {
        private readonly ChatClient _client = new();
        private bool _isAuthorized = false;
        private const bool NeedLogs = true;

        public FormClient()
        {
            InitializeComponent();
            _client.ReadyToUse += OnReadyToUse;
            _client.AuthenticationError += OnAuthenticationError;
            _client.ServerIsUnreachable += OnServerUnreachable;
            if (NeedLogs)
            {
                _client.LogThis += OnLog;
            }

            SwitchToConnectionInterface();
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

        private void OnReadyToUse(ConnectionEventArgs e)
        {
            SubscribeClientChatEvents();
            SwitchToChatInterface();
            _isAuthorized = true;
            Log($"*** Server {e.IpPort} connected", LogsColors.SystemConnected);
        }

        private void OnAuthenticationError(AuthenticationErrorEventArgs e)
        {
            _isAuthorized = false;
            Log(
                $"*** Server {e.IpPort} rejected authentication: {Enum.GetName(typeof(AnswerOnAuthenticationTypes), e.AuthenticationError)}",
                LogsColors.SystemConnected);
        }

        private void OnServerUnreachable(ConnectionEventArgs e)
        {
            Log($"*** Server {e.IpPort} is unreachable", LogsColors.Message);
        }

        private void OnDisconnected(ConnectionEventArgs e)
        {
            Log($"*** Server {e.IpPort} disconnected", LogsColors.SystemDisconnected);
            _isAuthorized = false;
            UnsubscribeClientChatEvents();
            SwitchToConnectionInterface();
        }

        private void OnAnotherClientAuthorize(UserEventArgs e)
        {
            Log($"[{e.IpPort}] [{e.Nick}]: connected", LogsColors.SystemConnected);
        }

        private void OnAnotherClientDisconnected(UserEventArgs e)
        {
            Log($"[{e.IpPort}] [{e.Nick}]: disconnected", LogsColors.SystemDisconnected);
        }

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            Log($"[{e.IpPort}] [{e.Nick}]: {e.Message}", LogsColors.Message);
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
            panelConnection.Enabled = false;
            buttonDisconnect.Enabled = true;
            panelChat.Enabled = true;
        }

        private void SwitchToConnectionInterface()
        {
            panelConnection.Enabled = true;
            buttonDisconnect.Enabled = false;
            panelChat.Enabled = false;
        }

        private void UnsubscribeClientChatEvents()
        {
            _client.Disconnected -= OnDisconnected;
            _client.MessageReceived -= OnMessageReceived;
            _client.AnotherClientAuthorize -= OnAnotherClientAuthorize;
            _client.AnotherClientDisconnected -= OnAnotherClientDisconnected;
        }

        private void SubscribeClientChatEvents()
        {
            _client.Disconnected += OnDisconnected;
            _client.MessageReceived += OnMessageReceived;
            _client.AnotherClientAuthorize += OnAnotherClientAuthorize;
            _client.AnotherClientDisconnected += OnAnotherClientDisconnected;
        }

        private void TryAuthorize(bool needSignUp)
        {
            if (_isAuthorized || !NetworkTools.IsAddressAndPortCorrect(textBoxIP.Text, textBoxPort.Text) ||
                textBoxNick.Text == string.Empty || textBoxPassword.Text == string.Empty) return;
            _client.TryAuthorize(textBoxIP.Text + ":" + textBoxPort.Text, textBoxNick.Text, textBoxPassword.Text,
                needSignUp);
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            TryAuthorize(false);
        }

        private void buttonSignUp_MouseClick(object sender, MouseEventArgs e)
        {
            TryAuthorize(true);
        }

        private void richTextBoxChat_TextChanged(object sender, EventArgs e)
        {
            richTextBoxChat.SelectionStart = richTextBoxChat.Text.Length;
            richTextBoxChat.ScrollToCaret();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (!_isAuthorized) return;
            _client.Disconnect();
            UnsubscribeClientChatEvents();
            SwitchToConnectionInterface();
            _isAuthorized = false;
        }
    }
}