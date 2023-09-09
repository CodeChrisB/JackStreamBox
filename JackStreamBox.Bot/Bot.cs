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
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Logger;
using JackStreamBox.Util.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using DSharpPlus.SlashCommands;

using System.Text;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Voting;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Help;
using JackStreamBox.Bot.Logic.Commands.StaffCommand.ListSettings;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Pack;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Report;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Menu;
using JackStreamBox.Bot.Logic.Commands.UserCommands;
using JackStreamBox.Bot.Logic.Commands._Helper.Ascii;
using JackStreamBox.Bot.Logic.Scheduled.Overwatch;

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


            //Create Logger
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Sink(new CustomLogSink(JackBotLogger.OnLog))
            .CreateLogger();


            var logFactory = new LoggerFactory().AddSerilog();

            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                //LoggerFactory = logFactory

            };

            Client = new DiscordClient(config);
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(1)
            });

            Client.ComponentInteractionCreated += async (s, e) => await ButtonHandler.OnInteraction(s, e);


            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
            };

            Console.WriteLine("Set up - Complete");



            Commands = Client.UseCommandsNext(commandsConfig);
            var Slash = Client.UseSlashCommands();



            //text commands
            Console.WriteLine("Dsharp - Register Commands");
            Commands.RegisterCommands<StartGameCommand>();
            Commands.RegisterCommands<PackCommand>();
            Commands.RegisterCommands<VoteCommand>();
            Commands.RegisterCommands<HelpCommand>();
            Commands.RegisterCommands<CommandLevel>();
            Commands.RegisterCommands<JokeCommand>();
            Commands.RegisterCommands<InputCommand>();
            Commands.RegisterCommands<SayCommand>();
            Commands.RegisterCommands<UpdaterCommand>();
            Commands.RegisterCommands<ReportCommand>();
            Commands.RegisterCommands<SetValue>();
            Commands.RegisterCommands<BannerCommands>();
            Commands.RegisterCommands<RuleCommands>();
            Commands.RegisterCommands<ShowModCommand>();
            Commands.RegisterCommands<DailyQuestionCommand>();
            Commands.RegisterCommands<MenuCommand>();

            //Slash Commands
            Slash.RegisterCommands<HelpSlash>();
            Slash.RegisterCommands<PackSlash>();
            Slash.RegisterCommands<ReportSlash>();
            Slash.RegisterCommands<VoteSlash>();
            Slash.RegisterCommands<MenuSlash>();


            //Register for Help Page
            BotCommand.Register<StartGameCommand>();
            BotCommand.Register<PackCommand>();
            BotCommand.Register<VoteCommand>();
            BotCommand.Register<HelpCommand>();
            BotCommand.Register<JokeCommand>();
            BotCommand.Register<InputCommand>();
            BotCommand.Register<CommandLevel>();
            BotCommand.Register<SayCommand>();
            BotCommand.Register<UpdaterCommand>();
            BotCommand.Register<MenuCommand>();
            BotCommand.Register<ReportCommand>();
            BotCommand.Register<SetValue>();
            BotCommand.Register<BannerCommands>();
            BotCommand.Register<RuleCommands>();

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


            Console.WriteLine("Setting up - Scheduler");
            OverwatchVC.guildId = 697504479834275898;
            Scheduler.RegisterScheduler("XPDistribution", async () =>
            {
                await OverwatchVC.DistributeXP();
            }, TimeSpan.FromMinutes(10));

            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
