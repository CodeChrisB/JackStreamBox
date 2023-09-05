using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using DSharpPlus.Entities;
using DSharpPlus;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Menu
{
    internal class MenuSlash : ApplicationCommandModule
    {
        [SlashCommand("menu", "Opens a menu that lets you vote for all packs")]
        public async Task GetAll(InteractionContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Used **/menu**"));

            MenuLogic.OpenMenu(context.ToCustomContext());
        }
    }
}
