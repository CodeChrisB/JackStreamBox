using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper
{
    public class CustomContext
    {
        public DiscordChannel Channel { get; set; }
        public DiscordMessage Message { get; set; }
        public DiscordUser User { get; set; }
        public DiscordMember Member { get; set; }
        public DiscordClient Client { get; set; }
        public DiscordGuild Guild { get; set; }

        public CustomContext(CommandContext context) { 
            Guild = context.Guild;
            Channel = context.Channel;
            Message = context.Message;
            User = context.User;
            Member = context.Member;
            Client = context.Client;
        }

        public CustomContext(InteractionContext context)
        {
            Guild = context.Guild;
            Channel = context.Channel;
            Message = null;
            User = context.User;
            Member = context.Member;
            Client = context.Client;
        }

        public CustomContext(ComponentInteractionCreateEventArgs context)
        {
            Guild = context.Guild;
            Channel = context.Interaction.Channel;
            Message = null;
            User = context.Interaction.User;
            Member = (DiscordMember)context.User;
            Client = Bot.Client;   
        }
    }
}
