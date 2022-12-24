namespace Server
{
    partial class FormServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonServerStatusChange = new System.Windows.Forms.Button();
            this.panelConnection = new System.Windows.Forms.Panel();
            this.labelNick = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxNick = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.panelChat = new System.Windows.Forms.Panel();
            this.textBoxMessageInput = new System.Windows.Forms.TextBox();
            this.richTextBoxChat = new System.Windows.Forms.RichTextBox();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.panelConnection.SuspendLayout();
            this.panelChat.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonServerStatusChange
            // 
            this.buttonServerStatusChange.Location = new System.Drawing.Point(633, 18);
            this.buttonServerStatusChange.Name = "buttonServerStatusChange";
            this.buttonServerStatusChange.Size = new System.Drawing.Size(136, 23);
            this.buttonServerStatusChange.TabIndex = 6;
            this.buttonServerStatusChange.Text = "Start";
            this.buttonServerStatusChange.UseVisualStyleBackColor = true;
            this.buttonServerStatusChange.Click += new System.EventHandler(this.buttonServerStatusChange_Click);
            // 
            // panelConnection
            // 
            this.panelConnection.Controls.Add(this.labelNick);
            this.panelConnection.Controls.Add(this.label2);
            this.panelConnection.Controls.Add(this.labelIP);
            this.panelConnection.Controls.Add(this.textBoxNick);
            this.panelConnection.Controls.Add(this.textBoxPort);
            this.panelConnection.Controls.Add(this.textBoxIP);
            this.panelConnection.Location = new System.Drawing.Point(12, 12);
            this.panelConnection.Name = "panelConnection";
            this.panelConnection.Size = new System.Drawing.Size(615, 39);
            this.panelConnection.TabIndex = 7;
            // 
            // labelNick
            // 
            this.labelNick.Location = new System.Drawing.Point(440, 6);
            this.labelNick.Name = "labelNick";
            this.labelNick.Size = new System.Drawing.Size(41, 23);
            this.labelNick.TabIndex = 2;
            this.labelNick.Text = "Nick";
            this.labelNick.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(247, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIP
            // 
            this.labelIP.Location = new System.Drawing.Point(3, 6);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(29, 23);
            this.labelIP.TabIndex = 1;
            this.labelIP.Text = "IP:";
            this.labelIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNick
            // 
            this.textBoxNick.Location = new System.Drawing.Point(487, 6);
            this.textBoxNick.Name = "textBoxNick";
            this.textBoxNick.Size = new System.Drawing.Size(125, 20);
            this.textBoxNick.TabIndex = 0;
            this.textBoxNick.Text = "Server";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(294, 6);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(125, 20);
            this.textBoxPort.TabIndex = 0;
            this.textBoxPort.Text = "9000";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(38, 6);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(188, 20);
            this.textBoxIP.TabIndex = 0;
            this.textBoxIP.Text = "127.0.0.1";
            // 
            // panelChat
            // 
            this.panelChat.Controls.Add(this.textBoxMessageInput);
            this.panelChat.Controls.Add(this.richTextBoxChat);
            this.panelChat.Controls.Add(this.buttonSendMessage);
            this.panelChat.Enabled = false;
            this.panelChat.Location = new System.Drawing.Point(12, 57);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(765, 399);
            this.panelChat.TabIndex = 5;
            // 
            // textBoxMessageInput
            // 
            this.textBoxMessageInput.Location = new System.Drawing.Point(6, 367);
            this.textBoxMessageInput.Name = "textBoxMessageInput";
            this.textBoxMessageInput.Size = new System.Drawing.Size(671, 20);
            this.textBoxMessageInput.TabIndex = 1;
            this.textBoxMessageInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxMessageInput_KeyDown);
            // 
            // richTextBoxChat
            // 
            this.richTextBoxChat.Location = new System.Drawing.Point(6, 3);
            this.richTextBoxChat.Name = "richTextBoxChat";
            this.richTextBoxChat.ReadOnly = true;
            this.richTextBoxChat.Size = new System.Drawing.Size(751, 358);
            this.richTextBoxChat.TabIndex = 0;
            this.richTextBoxChat.Text = "";
            this.richTextBoxChat.TextChanged += new System.EventHandler(this.richTextBoxChat_TextChanged);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(682, 367);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(75, 20);
            this.buttonSendMessage.TabIndex = 2;
            this.buttonSendMessage.Text = "Send";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSendMessage_MouseClick);
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 461);
            this.Controls.Add(this.buttonServerStatusChange);
            this.Controls.Add(this.panelConnection);
            this.Controls.Add(this.panelChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormServer";
            this.Text = "Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormServer_FormClosed);
            this.panelConnection.ResumeLayout(false);
            this.panelConnection.PerformLayout();
            this.panelChat.ResumeLayout(false);
            this.panelChat.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox textBoxNick;
        private System.Windows.Forms.Label labelNick;

        private System.Windows.Forms.Button buttonServerStatusChange;
        private System.Windows.Forms.Panel panelConnection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.TextBox textBoxMessageInput;
        private System.Windows.Forms.RichTextBox richTextBoxChat;
        private System.Windows.Forms.Button buttonSendMessage;

        #endregion
    }
}