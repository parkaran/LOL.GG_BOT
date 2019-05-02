namespace BotTelegram {
    partial class frmMain {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            this.txtTelegramToken = new System.Windows.Forms.TextBox();
            this.txtRiotApiToken = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.lblTelegramToken = new System.Windows.Forms.Label();
            this.lblRiotApiToken = new System.Windows.Forms.Label();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.btnClearConsole = new System.Windows.Forms.Button();
            this.lblConsole = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTelegramToken
            // 
            this.txtTelegramToken.Location = new System.Drawing.Point(24, 22);
            this.txtTelegramToken.Name = "txtTelegramToken";
            this.txtTelegramToken.Size = new System.Drawing.Size(100, 20);
            this.txtTelegramToken.TabIndex = 0;
            // 
            // txtRiotApiToken
            // 
            this.txtRiotApiToken.Location = new System.Drawing.Point(24, 62);
            this.txtRiotApiToken.Name = "txtRiotApiToken";
            this.txtRiotApiToken.Size = new System.Drawing.Size(100, 20);
            this.txtRiotApiToken.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(184, 46);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(310, 46);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 3;
            this.BtnStop.Text = "Stop Server";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // lblTelegramToken
            // 
            this.lblTelegramToken.AutoSize = true;
            this.lblTelegramToken.Location = new System.Drawing.Point(21, 6);
            this.lblTelegramToken.Name = "lblTelegramToken";
            this.lblTelegramToken.Size = new System.Drawing.Size(84, 13);
            this.lblTelegramToken.TabIndex = 4;
            this.lblTelegramToken.Text = "Telegram token:";
            // 
            // lblRiotApiToken
            // 
            this.lblRiotApiToken.AutoSize = true;
            this.lblRiotApiToken.Location = new System.Drawing.Point(21, 46);
            this.lblRiotApiToken.Name = "lblRiotApiToken";
            this.lblRiotApiToken.Size = new System.Drawing.Size(81, 13);
            this.lblRiotApiToken.TabIndex = 5;
            this.lblRiotApiToken.Text = "Riot Api Token:";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(24, 116);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.Size = new System.Drawing.Size(599, 322);
            this.txtConsole.TabIndex = 6;
            this.txtConsole.Text = "";
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Location = new System.Drawing.Point(424, 46);
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Size = new System.Drawing.Size(75, 23);
            this.btnClearConsole.TabIndex = 7;
            this.btnClearConsole.Text = "Clear Console";
            this.btnClearConsole.UseVisualStyleBackColor = true;
            this.btnClearConsole.Click += new System.EventHandler(this.btnClearConsole_Click);
            // 
            // lblConsole
            // 
            this.lblConsole.AutoSize = true;
            this.lblConsole.Location = new System.Drawing.Point(24, 103);
            this.lblConsole.Name = "lblConsole";
            this.lblConsole.Size = new System.Drawing.Size(48, 13);
            this.lblConsole.TabIndex = 8;
            this.lblConsole.Text = "Console:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 450);
            this.Controls.Add(this.lblConsole);
            this.Controls.Add(this.btnClearConsole);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.lblRiotApiToken);
            this.Controls.Add(this.lblTelegramToken);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtRiotApiToken);
            this.Controls.Add(this.txtTelegramToken);
            this.Name = "frmMain";
            this.Text = "TelegramBot_SERVER";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTelegramToken;
        private System.Windows.Forms.TextBox txtRiotApiToken;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Label lblTelegramToken;
        private System.Windows.Forms.Label lblRiotApiToken;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.Button btnClearConsole;
        private System.Windows.Forms.Label lblConsole;
    }
}

