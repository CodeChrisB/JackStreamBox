using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Voting
{
    internal class VoteSlash : ApplicationCommandModule
    {

        [SlashCommand("1", "Vote for Pack 1")] 
        public async Task Vote1(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "1"); }
        [SlashCommand("2", "Vote for Pack 2")] 
        public async Task Vote2(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "2"); }
        [SlashCommand("3", "Vote for Pack 3")] 
        public async Task Vote3(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "3"); }
        [SlashCommand("4", "Vote for Pack 4")] 
        public async Task Vote4(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "4"); }
        [SlashCommand("5", "Vote for Pack 5")] 
        public async Task Vote5(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "5"); }
        [SlashCommand("6", "Vote for Pack 6")] 
        public async Task Vote6(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "6"); }
        [SlashCommand("7", "Vote for Pack 7")] 
        public async Task Vote7(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "7"); }
        [SlashCommand("8", "Vote for Pack 8")] 
        public async Task Vote8(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "8"); }
        [SlashCommand("9", "Vote for Pack 9")] 
        public async Task Vote9(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "9"); }
        [SlashCommand("10", "Vote for Pack 10")] 
        public async Task Vote10(InteractionContext context) { await VoteCommand.VoteViaSlash(context, "10"); }
    }
}
