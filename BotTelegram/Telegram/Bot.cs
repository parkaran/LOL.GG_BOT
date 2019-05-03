using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using BotTelegram.Delegates;
using Telegram.Bot.Types.Enums;
using RiotSharp;
using RiotSharp.Misc;
using RiotSharp.Endpoints.SpectatorEndpoint;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Endpoints.ChampionEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.ProfileIcons;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.StaticDataEndpoint;
using System.Text.RegularExpressions;

namespace BotTelegram.Telegram {
    public class Bot {

        private TelegramBotClient _botClient;
        private UtiliDelegate.Console ConsoleWrite;
        private RiotApi _lolApi;
        private Regex ValidateName = new Regex("/[0 - 9A - Za - zªµºÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿĂăĄąĆćĘęıŁłŃńŐőŒœŚśŞşŠšŢţŰűŸŹźŻżŽžƒȘșȚțˆˇˉΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩάέήίαβγδεζηθικλμνξοπρςστυφχψωόύώﬁﬂ] +$/ g");
        private ProfileIconListStatic _profileIcons = new ProfileIconListStatic();
        //private ChampionListStatic _championList = new ChampionListStatic();
        private string _latestVersion = ""; 
        public Bot(string TelegramToken, string RiotToken) {
            _botClient = new TelegramBotClient(TelegramToken);
            _lolApi = RiotApi.GetDevelopmentInstance(RiotToken);
            _botClient.OnMessage += _botClient_OnMessage;
            InitializeProfileIcons();
            //InitializeChampionList();

        }
        private async void InitializeProfileIcons() {
            var latestVersion = (await _lolApi.StaticData.Versions.GetAllAsync()).First();
            var test = await _lolApi.StaticData.ProfileIcons.GetAllAsync(latestVersion);
            _profileIcons.ProfileIcons = test.ProfileIcons;
            _latestVersion = latestVersion;
        }
        //private async void InitializeChampionList() {
            
        //}
        public void SetConsoleReferance(UtiliDelegate.Console consoleRef) {
            ConsoleWrite = consoleRef;
        }

        private async void _botClient_OnMessage(object sender, MessageEventArgs e) {
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;

            ConsoleWrite($"{DateTime.Now.ToLongTimeString()} {e.Message.Chat.FirstName} sent a message", false);

            switch (e.Message.Text.Split(' ').First()) {

                case "/start":
                    SendMessageAsync($"Welcome {e.Message.Chat.FirstName}, Use /help to get more information", e.Message.Chat);
                    break;
                case "/help":
                    SendMessageAsync(HelpCommand(), e.Message.Chat);
                    break;
                case "/name":
                    string[] SummonerName = e.Message.Text.Split(' ');
                    if (SummonerName.Length != 2) { SendMessageAsync("Invalid Command...", e.Message.Chat); return; };
                    string name = SummonerName[1].Replace("/", " ").ToLower();

                    Summoner summ = await _lolApi.Summoner.GetSummonerByNameAsync(Region.Euw, name);
                    ProfileIconStatic icon;

                    bool userProfileIcon = _profileIcons.ProfileIcons.TryGetValue(summ.ProfileIconId.ToString(), out icon);
                    if (userProfileIcon)
                        await SendProfileIconAsync("http://ddragon.leagueoflegends.com/cdn/" + _latestVersion + "/img/profileicon/" + icon.Image.Fulls, e.Message.Chat);

                    //var data = Newtonsoft.Json.JsonConvert.SerializeObject(summ, Newtonsoft.Json.Formatting.Indented);
                    string moreinfo = await SummonerIsInAGameAsync(summ.Id);
                    SendMessageAsync($" Name: {summ.Name}\n Level: {summ.Level}\n Status: {moreinfo}", e.Message.Chat);
                    break;
                case "/free":
                    GetCurrentRotationAsync();
                    break;
                case "/mastery":

                    break;
                default:
                    SendMessageAsync("Invalid Command, try using /help to get more information", e.Message.Chat);
                    break;
            }
               
        }
        private async Task<string> GetCurrentRotationAsync() {
            StringBuilder text = new StringBuilder(50);
            ChampionRotation currentRotaion = await _lolApi.Champion.GetChampionRotationAsync(Region.Euw);
            text.Append("Free champions:\n<=========>\n");
            foreach (var item in currentRotaion.FreeChampionIds) {
                ChampionStatic champ = await _lolApi.StaticData.Champions.GetByKeyAsync(item.ToString(), _latestVersion);
                text.Append();
            }
            


               return output;
        }
        private async Task<string> SummonerIsInAGameAsync(string SummonerId) {
            string output = "";
            CurrentGame test;
            try {
                 test = await _lolApi.Spectator.GetCurrentGameAsync(Region.Euw, SummonerId);
            }
            catch(Exception ex) {
                return "Currently not in game";
            }
            output = $"In a {test.GameMode} game\n Map Type: {test.MapType}\n Game Time: {test.GameLength.Minutes}";

            return output;
        }
        private string HelpCommand() { 
            string[] Comm = new string[]{   "/name 'Summoner_Name'--> get information on the EU summoners",
                                            "/free --> get free available champions",
                                            "/mastery --> no info yet..."};
            string output ="";
            foreach (var com in Comm) {
                output += com + "\n";
            }
            return output;
        }
        private async void SendMessageAsync(string msg, Chat SendTochat) {
             _botClient.SendTextMessageAsync(
                      chatId: SendTochat,
                      text: msg
                    );
            
        }
        private async Task SendProfileIconAsync(string image, Chat SendTochat) {
             _botClient.SendPhotoAsync(
                     chatId: SendTochat,
                     photo: image,
                     caption: "Profile Icon"
                   );
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
