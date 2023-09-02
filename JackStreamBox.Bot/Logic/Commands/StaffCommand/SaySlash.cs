using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    internal class SaySlash : ApplicationCommandModule
    {
        [SlashCommand("test", "A slash command made to test the DSharpPlusSlashCommands library!")]
        public async Task TestCommand(InteractionContext context) {
            await context.Channel.SendMessageAsync("Does Slashing works?");
        }
    }
}
