using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class PackCommand : BaseCommandModule
    {
        [Command("pack")]
        public async Task DisplayPack(CommandContext context, int pack)
        {
            if (pack > 0 && pack < 10)
            {
                DiscordMessage command = context.Message;
                string[] emoji = new string[5] {
                ":one:",
                ":two:",
                ":three:",
                ":four:",
                ":five:"
                };

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Pack {pack} contains following games");
                Pack packInfo = PackInfo.GetPackInfo(pack);
                for (int i = 0; i < 5; i++)
                {
                    stringBuilder.AppendLine($"{emoji[i]} {packInfo.games[i].Name}");
                    stringBuilder.AppendLine($"  -> {packInfo.games[i].Description}");

                }

                await context.Channel.SendMessageAsync(stringBuilder.ToString());
            }
        }

        [Command("pack")]
        public async Task DisplayPack(CommandContext context)
        {
            await context.Channel.SendMessageAsync("Try !pack 1");
        }
    }
}
