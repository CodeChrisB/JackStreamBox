using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using JackStreamBox.Bot.Logic.Commands;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands.DevCommands;
using JackStreamBox.Bot.Logic.Commands.ScheduledCommands;
using JackStreamBox.Bot.Logic.Commands.StaffCommand;
using JackStreamBox.Bot.Logic.Commands.UserCommands;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JackStreamBox.Bot
{
    public class Bot
    {
        public static DiscordClient Client {  get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public static bool CRASHED { get; set; }


        public async Task RunAsync()
        {

            Console.WriteLine("*********************************");
            Console.WriteLine("   __     ______     __    __    \r\n  /\\ \\   /\\  ___\\   /\\ \"-./  \\   \r\n _\\_\\ \\  \\ \\___  \\  \\ \\ \\-./\\ \\  \r\n/\\_____\\  \\/\\_____\\  \\ \\_\\ \\ \\_\\ \r\n\\/_____/   \\/_____/   \\/_/  \\/_/ \r\n                                 ");
            Console.WriteLine("*********************************");

            Console.WriteLine("Loading - Config");
            var json = String.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();
            var configJson = JsonConvert.DeserializeObject<Config>(json);

            Console.WriteLine("Loading - Done!");
            Console.WriteLine("Set up - Dsharp Bot");


            DocGenerator.PASTE_BIN_KEY = configJson.PasteBinKey;

            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true

            };

            Client = new DiscordClient(config);
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(1)
            });

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
            };

            Console.WriteLine("Set up - Complete");



            Commands = Client.UseCommandsNext(commandsConfig);

            //text commands
            Commands.RegisterCommands<StartGameCommand>();
            Commands.RegisterCommands<PackCommand>();
            Commands.RegisterCommands<VotingCommand>();
            Commands.RegisterCommands<HelpCommand>();
            Commands.RegisterCommands<CommandLevel>();
            Commands.RegisterCommands<JokeCommand>();
            Commands.RegisterCommands<InputCommand>();
            Commands.RegisterCommands<SayCommand>();
            Commands.RegisterCommands<UpdaterCommand>();
            Commands.RegisterCommands<ReportCommand>();
            Commands.RegisterCommands<SetValue>();
            Commands.RegisterCommands<BannerCommands>();
            Commands.RegisterCommands<ShowModCommand>();
            Commands.RegisterCommands<DailyQuestionCommand>();

            Console.WriteLine("Dsharp - Register Commands");
            
            //Register for Help Page
            BotCommand.Register<StartGameCommand>();
            BotCommand.Register<PackCommand>();
            BotCommand.Register<VotingCommand>();
            BotCommand.Register<HelpCommand>();
            BotCommand.Register<JokeCommand>();
            BotCommand.Register<InputCommand>();
            BotCommand.Register<CommandLevel>();
            BotCommand.Register<SayCommand>();
            BotCommand.Register<UpdaterCommand>();
            BotCommand.Register<ReportCommand>();
            BotCommand.Register<SetValue>();
            BotCommand.Register<BannerCommands>();

            Console.WriteLine("Reflection - Register Commands");


            //Register for ModCommand
            Console.WriteLine("Reflection - Register Fake Mod Commands");
            BotCommand.Register<ShowModCommand>();
            BotCommand.Register<DailyQuestionCommand>();


            //Generate Command Markdown
            Console.WriteLine("Generator - Check for changes in Commands");

            await BotCommand.GenerateMarkdown();


            //Load Settings
            Console.WriteLine("Loading - Bot Settings");
            BotData.LoadBotSetings();
            Console.WriteLine("Loading - Complete");



            Console.WriteLine("*********************************");
            //Set Bot Mode
            string name = BotData.ReadData(BotVals.BOT_NAME, "TB1");
            CommandLevel.IsDevBot = name.ToLower().Contains("dev");

            //Connect to Discord
            await Client.ConnectAsync();

            //Send Welcome Message
            AsciiArt.WelcomeMessage(Client);
            Console.WriteLine("Sent - Log sent Message");



            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
