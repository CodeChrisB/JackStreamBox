using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands
{
    public class StartGame : BaseCommandModule
    {
        [Command("start")]
        public async Task OpenGame(CommandContext context)
        {
            await context.Channel.SendMessageAsync("Test");
            var task = JackStreamBoxUtility.OpenGame(Util.Data.Game.Jobjob);
            await task;
        }

        [Command("pack")]
        public async Task DisplayPack(CommandContext context,int pack)
        {
            if(pack>0 && pack < 10)
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
