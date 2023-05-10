﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using JackStreamBox.Bot.Logic.Commands;
using JackStreamBox.Bot.Logic.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Timeout = TimeSpan.FromMinutes(2)
            });

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<StartGameCommand>();
            Commands.RegisterCommands<PackCommand>();
            Commands.RegisterCommands<VotingCommand>();
            Commands.RegisterCommands<HelpCommand>();
            Commands.RegisterCommands<CommandLevel>();


            BotCommand.Register<StartGameCommand>();
            BotCommand.Register<PackCommand>();
            BotCommand.Register<VotingCommand>();
            BotCommand.Register<HelpCommand>();
            BotCommand.Register<CommandLevel>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
