using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using BotTelegram.Delegates;


namespace BotTelegram.Telegram {
    public class Bot {

        private TelegramBotClient _botClient;
        private UtiliDelegate.Console ConsoleWrite;

        public Bot(string Token) {
            _botClient = new TelegramBotClient(Token);
            _botClient.OnMessage += _botClient_OnMessage;

        }
        public void SetConsoleReferance(UtiliDelegate.Console consoleRef) {
            ConsoleWrite = consoleRef;
        }

        private async void _botClient_OnMessage(object sender, MessageEventArgs e) {
            if (e.Message.Text != null) {
                //Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                if (e.Message.Text != "/start") {
                    await _botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "You said:\n" + e.Message.Text
                    );
                    ConsoleWrite($"{DateTime.Now.ToLongTimeString()} {e.Message.Chat.FirstName} sent a message", false);
                }
                else {
                    await _botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "Welcome: " + e.Message.Chat.FirstName
                    );
                    ConsoleWrite($"{DateTime.Now.ToLongTimeString()} {e.Message.Chat.FirstName} Welcome message sent", false);
                }
                
            }

        }

        public void StartReceiving() {
            try {
                _botClient.StartReceiving();
                ConsoleWrite("Bot has been started", false);
            }
            catch (Exception ex) {
                ConsoleWrite(ex.Message, true);
            }
        }
        public void StopReceiving() {
            try {
                _botClient.StopReceiving();
                ConsoleWrite("Bot has been stoped", false);
            }
            catch (Exception ex) {
                ConsoleWrite(ex.Message, true);
            }
        }
    }
}
