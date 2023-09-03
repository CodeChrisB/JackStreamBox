using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Voting
{
    internal class VoteSlash : ApplicationCommandModule
    {
        [SlashCommand("vote", "Vote for a pack")]
        public async Task Vote(InteractionContext ctx,
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
            [Option("pack", "The pack you want to vote for.")] long pack  = -1)
        {
            // Call your VoteLogic.VoteViaSlash function with the selected pack
            await VoteLogic.VoteViaSlash(ctx, pack.ToString());

            if(pack >0) {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You voted for {pack.ToString()}"));
            }
            // Respond to the interaction

        }


    }
}
