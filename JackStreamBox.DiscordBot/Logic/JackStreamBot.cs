using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.DiscordBot.Logic.Commands;
using JackStreamBox.DiscordBot.Logic.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace JackStreamBox.DiscordBot.Logic
{
    internal class JackStreamBot
    {
        public JackStreamBot() {}

        public async Task<bool> SetUp()
        {
            //Set Up the Bot
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", false, true)
                    .Build();

                    app.AddConfiguration(configuration);
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .ConfigureDiscordHost((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = Discord.LogSeverity.Debug,
                        AlwaysDownloadUsers = false,
                        MessageCacheSize = 200
                    };

                    config.Token = context.Configuration["Token"];

                })
                .UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = Discord.LogSeverity.Debug;
                    config.DefaultRunMode = RunMode.Sync;
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHostedService<CommandHandler>();
                });
                


            return true;
        }

        public
    }
}
