namespace Client
{
    partial class FormClient
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
            this.richTextBoxChat = new System.Windows.Forms.RichTextBox();
            this.textBoxMessageInput = new System.Windows.Forms.TextBox();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.panelChat = new System.Windows.Forms.Panel();
            this.panelConnection = new System.Windows.Forms.Panel();
            this.buttonSignUp = new System.Windows.Forms.Button();
            this.buttonLogIn = new System.Windows.Forms.Button();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelNick = new System.Windows.Forms.Label();
            this.textBoxNick = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.panelChat.SuspendLayout();
            this.panelConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxChat
            // 
            this.richTextBoxChat.Location = new System.Drawing.Point(6, 6);
            this.richTextBoxChat.Name = "richTextBoxChat";
            this.richTextBoxChat.ReadOnly = true;
            this.richTextBoxChat.Size = new System.Drawing.Size(722, 388);
            this.richTextBoxChat.TabIndex = 0;
            this.richTextBoxChat.Text = "";
            this.richTextBoxChat.TextChanged += new System.EventHandler(this.richTextBoxChat_TextChanged);
            // 
            // textBoxMessageInput
            // 
            this.textBoxMessageInput.Location = new System.Drawing.Point(3, 403);
            this.textBoxMessageInput.Name = "textBoxMessageInput";
            this.textBoxMessageInput.Size = new System.Drawing.Size(641, 20);
            this.textBoxMessageInput.TabIndex = 1;
            this.textBoxMessageInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxMessageInput_KeyDown);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(657, 400);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(75, 23);
            this.buttonSendMessage.TabIndex = 2;
            this.buttonSendMessage.Text = "Send";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSendMessage_MouseClick);
            // 
            // panelChat
            // 
            this.panelChat.Controls.Add(this.textBoxMessageInput);
            this.panelChat.Controls.Add(this.richTextBoxChat);
            this.panelChat.Controls.Add(this.buttonSendMessage);
            this.panelChat.Enabled = false;
            this.panelChat.Location = new System.Drawing.Point(12, 12);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(735, 426);
            this.panelChat.TabIndex = 3;
            // 
            // panelConnection
            // 
            this.panelConnection.Controls.Add(this.buttonSignUp);
            this.panelConnection.Controls.Add(this.buttonLogIn);
            this.panelConnection.Controls.Add(this.labelPassword);
            this.panelConnection.Controls.Add(this.textBoxPassword);
            this.panelConnection.Controls.Add(this.labelNick);
            this.panelConnection.Controls.Add(this.textBoxNick);
            this.panelConnection.Controls.Add(this.label2);
            this.panelConnection.Controls.Add(this.labelIP);
            this.panelConnection.Controls.Add(this.textBoxPort);
            this.panelConnection.Controls.Add(this.textBoxIP);
            this.panelConnection.Location = new System.Drawing.Point(753, 12);
            this.panelConnection.Name = "panelConnection";
            this.panelConnection.Size = new System.Drawing.Size(249, 137);
            this.panelConnection.TabIndex = 4;
            // 
            // buttonSignUp
            // 
            this.buttonSignUp.Location = new System.Drawing.Point(125, 107);
            this.buttonSignUp.Name = "buttonSignUp";
            this.buttonSignUp.Size = new System.Drawing.Size(118, 26);
            this.buttonSignUp.TabIndex = 4;
            this.buttonSignUp.Text = "Sign up";
            this.buttonSignUp.UseVisualStyleBackColor = true;
            this.buttonSignUp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSignUp_MouseClick);
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.Location = new System.Drawing.Point(3, 107);
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(116, 26);
            this.buttonLogIn.TabIndex = 5;
            this.buttonLogIn.Text = "Log in";
            this.buttonLogIn.UseVisualStyleBackColor = true;
            this.buttonLogIn.Click += new System.EventHandler(this.buttonLogIn_Click);
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(8, 81);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(60, 23);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Password";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(71, 81);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(172, 20);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.Text = "Password";
            // 
            // labelNick
            // 
            this.labelNick.Location = new System.Drawing.Point(27, 55);
            this.labelNick.Name = "labelNick";
            this.labelNick.Size = new System.Drawing.Size(41, 23);
            this.labelNick.TabIndex = 4;
            this.labelNick.Text = "Nick";
            this.labelNick.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNick
            // 
            this.textBoxNick.Location = new System.Drawing.Point(71, 55);
            this.textBoxNick.Name = "textBoxNick";
            this.textBoxNick.Size = new System.Drawing.Size(172, 20);
            this.textBoxNick.TabIndex = 3;
            this.textBoxNick.Text = "Client";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(36, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIP
            // 
            this.labelIP.Location = new System.Drawing.Point(36, 3);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(29, 23);
            this.labelIP.TabIndex = 1;
            this.labelIP.Text = "IP:";
            this.labelIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(71, 29);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(172, 20);
            this.textBoxPort.TabIndex = 0;
            this.textBoxPort.Text = "9000";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(71, 3);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(172, 20);
            this.textBoxIP.TabIndex = 0;
            this.textBoxIP.Text = "127.0.0.1";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(756, 151);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(240, 23);
            this.buttonDisconnect.TabIndex = 7;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 450);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.panelConnection);
            this.Controls.Add(this.panelChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormClient";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClient_FormClosed);
            this.panelChat.ResumeLayout(false);
            this.panelChat.PerformLayout();
            this.panelConnection.ResumeLayout(false);
            this.panelConnection.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Button buttonLogIn;

        private System.Windows.Forms.TextBox textBoxNick;
        private System.Windows.Forms.Label labelNick;

        private System.Windows.Forms.RichTextBox richTextBoxChat;
        private System.Windows.Forms.Panel panelChat;

        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSignUp;

        private System.Windows.Forms.Panel panelConnection;

        private System.Windows.Forms.TextBox textBoxMessageInput;
        private System.Windows.Forms.Button buttonSendMessage;

        private System.Windows.Forms.RichTextBox richTextBoxLogs;

        #endregion
    }
}