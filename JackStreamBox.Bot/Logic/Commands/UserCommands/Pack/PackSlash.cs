using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands.UserCommands.PackCommand;
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

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Pack
{
    internal class PackSlash : ApplicationCommandModule
    {
        [SlashCommand("pack","Views info about a pack")]
        public async Task DisplayPack(InteractionContext context, 
            [Choice("Pack 1", 1)]
            [Choice("Pack 2", 2)]
            [Choice("Pack 3", 3)]
            [Choice("Pack 4", 4)]
            [Choice("Pack 5", 5)]
            [Choice("Pack 6", 6)]
            [Choice("Pack 7", 7)]
            [Choice("Pack 8", 8)]
            [Choice("Pack 9", 9)]
            [Choice("Pack 10", 10)]
            [Option("pack", "Number of days of message history to delete")] long pack  = -1)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE, true)) return;
            PackLogic.DisplayPack(context.ToCustomContext(), (int)pack);

            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder());
        }

        //DO NOT ADD REQUIRES ATTRIBUTE OTHERWISE IT WILL SHOWUP IN THE HELP COMMAND
        [SlashCommand("packs","Get a list games and their respected player count")]
        public async Task DisplayPack(InteractionContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE, true)) return;
            PackLogic.SendPackScreenshot(context.ToCustomContext());
        }
    }
}
