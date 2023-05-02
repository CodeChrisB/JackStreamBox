
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JackStreamBox.DiscordBot.Logic.Commands
{
    public class CommandHandler : DiscordShardedClient
    {
        private readonly IServiceProvider Provider;
        private readonly DiscordSocketClient Client;
        private readonly CommandService Service;
        private readonly IConfiguration Configuration;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration configuration)
        {
            Provider = provider;
            Client = client;
            Service = service;
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.Client.MessageReceived += OnUserMessageRecieved;
        }

        private async Task OnUserMessageRecieved(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message)) return;
            if( message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(this.Configuration["Prefix"], ref argPos) && !message.HasMentionPrefix(this.Client.CurrentUser,ref argPos)) return;

            var context = new SocketCommandContext(this.Client, message);
            await this.Service.ExecuteAsync(context, argPos, this.Provider);
        }
    }
}
