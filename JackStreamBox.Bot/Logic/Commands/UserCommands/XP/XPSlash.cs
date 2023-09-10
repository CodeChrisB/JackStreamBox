using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Voting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Scheduled.Overwatch;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.XP
{
    internal class XPSlash : ApplicationCommandModule
    {
        [SlashCommand("hostxp", "Checkout Host XP")]
        public async Task Vote(InteractionContext ctx,
            [Choice("Your XP", 1)]
            [Choice("Top 10", 2)]
            [Option("Type", "Indicate what you want to see")] long ActionType  = -1)
        {
            //if (!CommandLevel.CanExecuteCommand(ctx.ToCustomContext(), PermissionRole.ANYONE,true)) return;
            // Call your VoteLogic.VoteViaSlash function with the selected pack

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Used **/hostxp**"));

            if (ActionType == 1) XPCommandLogic.ShowOwnXP(ctx);
            else XPCommandLogic.ShowTopXP(ctx);



            // Respond to the interaction

        }

     
    }
}
