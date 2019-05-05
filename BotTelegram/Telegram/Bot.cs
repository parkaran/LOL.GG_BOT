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
using System.Net;

namespace BotTelegram.Telegram {
    public class Bot {

        private TelegramBotClient _botClient;
        private UtiliDelegate.Console ConsoleWrite;
        private RiotApi _lolApi;
        private Regex ValidateName = new Regex("/[0 - 9A - Za - zªµºÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿĂăĄąĆćĘęıŁłŃńŐőŒœŚśŞşŠšŢţŰűŸŹźŻżŽžƒȘșȚțˆˇˉΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩάέήίαβγδεζηθικλμνξοπρςστυφχψωόύώﬁﬂ] +$/ g");
        private ProfileIconListStatic _profileIcons = new ProfileIconListStatic();
        private StaticChampionData _championList;
        private string _latestVersion = ""; 
        public Bot(string TelegramToken, string RiotToken) {
            //Riot API
            _lolApi = RiotApi.GetDevelopmentInstance(RiotToken);
            InizializeRiotApiAsync();


            //Telegram
            _botClient = new TelegramBotClient(TelegramToken);
            _botClient.OnMessage += _botClient_OnMessage;          
        }
        private async Task InizializeRiotApiAsync() {
            await InitializeProfileIcons();
            InitializeChampionList();
        }
        private async Task InitializeProfileIcons() {
            var latestVersion = (await _lolApi.StaticData.Versions.GetAllAsync()).First();
            var test = await _lolApi.StaticData.ProfileIcons.GetAllAsync(latestVersion);
            _profileIcons.ProfileIcons = test.ProfileIcons;
            _latestVersion = latestVersion;
        }
        private async void InitializeChampionList() {
            _championList = new StaticChampionData();
            var json = new WebClient().DownloadString("http://ddragon.leagueoflegends.com/cdn/"+_latestVersion+"/data/en_US/champion.json");
            var champs = Newtonsoft.Json.JsonConvert.DeserializeObject<StaticChampionData>(json);
            _championList = champs;
            
        }
        
        public void SetConsoleReferance(UtiliDelegate.Console consoleRef) {
            ConsoleWrite = consoleRef;
        }

        private async void _botClient_OnMessage(object sender, MessageEventArgs e) {
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;

            ConsoleWrite($"{DateTime.Now.ToLongTimeString()} {e.Message.Chat.FirstName} sent a message", false);
            e.Message.Text = e.Message.Text.Trim();
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
                    Summoner summ = null; ;
                    try {
                        summ = await _lolApi.Summoner.GetSummonerByNameAsync(Region.Euw, name);
                    }
                    catch (Exception) {
                        SendMessageAsync($"'{name}' is invalid or dose not exist on the EUW server...",e.Message.Chat);
                        break;
                    }
                    ProfileIconStatic icon;

                    bool userProfileIcon = _profileIcons.ProfileIcons.TryGetValue(summ.ProfileIconId.ToString(), out icon);
                    if (userProfileIcon)
                        await SendProfileIconAsync("http://ddragon.leagueoflegends.com/cdn/" + _latestVersion + "/img/profileicon/" + icon.Image.Full, e.Message.Chat);

                    //var data = Newtonsoft.Json.JsonConvert.SerializeObject(summ, Newtonsoft.Json.Formatting.Indented);
                    string moreinfo = await SummonerIsInAGameAsync(summ.Id);
                    SendMessageAsync($" Name: {summ.Name}\n Level: {summ.Level}\n Status: {moreinfo}", e.Message.Chat);
                    break;
                case "/free":
                    string Rotations = await GetCurrentRotationAsync();
                    SendMessageAsync($"{Rotations}",e.Message.Chat);
                    break;
                case "/champ":
                    string[] champName = e.Message.Text.Split(' ');
                    if (champName.Length != 2) { SendMessageAsync("Invalid Command, try using /help to get more information", e.Message.Chat); return; };
                    ChampionData champinfo = GetChampionByName(champName[1].ToLower());
                    if (champinfo != null) {
                        await SendProfileIconAsync("http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + champinfo.Name+ "_0.jpg", e.Message.Chat);
                        SendMessageAsync($"{champinfo.Name}\nAttack: {champinfo.Info.Attack}\nDefese: {champinfo.Info.Defense}\nMagic: {champinfo.Info.Magic}\nDifficulty: {champinfo.Info.Difficulty}/10\nBlurb: {champinfo.Blurb}", e.Message.Chat);
                    }
                    else
                        SendMessageAsync("Invalid Champion name... sorry", e.Message.Chat);
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
                ChampionData champ = GetChampionById(item);
                text.Append($"-♠- {champ.Name} <-> Champ difficulty: {champ.Info.Difficulty}/10\n");
            }
            text.Append("\n<=========>\n");
            text.Append("\nFree champions for new Players:\n<=========>\n");
            foreach (var item in currentRotaion.FreeChampionIdsForNewPlayers) {
                ChampionData champ = GetChampionById(item);
                text.Append($"-♠- {champ.Name} <-> Champ difficulty: {champ.Info.Difficulty}/10\n");
            }
            text.Append("\n<=========>\n");
            
            
            return text.ToString();
        }
        private ChampionData GetChampionById(int id) {
            var champ = _championList.Data.First(x => x.Value.Key == id);
            return champ.Value;         
        }
        private ChampionData GetChampionByName(string ChampName) {
            KeyValuePair<string,ChampionData> champ;
            try {
                champ = _championList.Data.First(x => x.Value.Name.ToLower().Remove(' ') == ChampName);
            }
            catch (Exception) {
                return null;
            }
            return champ.Value;
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
            output = $"In a {test.GameMode} game\n Map Type: {test.MapType}\n Game Time: {test.GameLength.Minutes} min";

            return output;
        }
        private string HelpCommand() { 
            string[] Comm = new string[]{   "/name 'Summoner_Name'--> get information on the EU summoners",
                                            "/free --> get free available champions",
                                            "/champ 'Champ_Name' --> get info on the champion"};
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
            await _botClient.SendPhotoAsync(
                     chatId: SendTochat,
                     photo: image
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
