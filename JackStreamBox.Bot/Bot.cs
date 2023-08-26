using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using JackStreamBox.Bot.Logic.Commands;
using JackStreamBox.Bot.Logic.Commands.DevCommands;
using JackStreamBox.Bot.Logic.Commands.ScheduledCommands;
using JackStreamBox.Bot.Logic.Commands.StaffCommand;
using JackStreamBox.Bot.Logic.Commands.UserCommands;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Util.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JackStreamBox.Bot
{
    public class Bot
    {
        public static DiscordClient Client {  get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }


        public async Task RunAsync()
        {
            var json = String.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();
            var configJson = JsonConvert.DeserializeObject<Config>(json);

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


            //Register for ModCommand
            BotCommand.Register<ShowModCommand>();
            BotCommand.Register<DailyQuestionCommand>(); 


            //Load Settings
            BotData.LoadBotSetings();
            //Send Online Message
            await Client.ConnectAsync();
            var channel = await Client.GetChannelAsync(ChannelId.JackBotVC);
            string name = BotData.ReadData(BotVals.BOT_NAME, "TB1");
            await channel.SendMessageAsync($"Hey JackStreamBox [{name}] is now online!");

            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
