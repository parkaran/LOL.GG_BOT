using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotTelegram.Telegram;

namespace BotTelegram {
    public partial class frmMain : Form {
        Bot TelegramBot;
        public frmMain() {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e) {
            txtTelegramToken.Text = "";
            txtRiotApiToken.Text = "";
        }

        private void btnStart_Click(object sender, EventArgs e) {
            TelegramBot = new Bot(txtTelegramToken.Text);
            TelegramBot.SetConsoleReferance(ConsoleWrite);
            TelegramBot.StartReceiving();
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            TelegramBot.StopReceiving();
            TelegramBot = null;
            
        }

        private void btnClearConsole_Click(object sender, EventArgs e) {
            txtConsole.Clear();
        }
        public void ConsoleWrite(string text, bool IsErrorMsg) {
            txtConsole.Invoke((MethodInvoker)delegate {
                if (IsErrorMsg)
                    txtConsole.SelectionColor = Color.Red;
                    txtConsole.AppendText(text+"\n");
                if (IsErrorMsg)
                    txtConsole.SelectionColor = Color.Black;
            });
        }
    }
}
