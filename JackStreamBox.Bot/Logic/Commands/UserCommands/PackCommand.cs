﻿using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Attributes;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class PackCommand : BaseCommandModule
    {
        [Command("pack")]
        [CoammandDescription("View a pack and it's games. **E.g !pack 5**",":mag:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayPack(CommandContext context, int pack)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE,true)) return;
            if (pack > 0 && pack < 10)
            {
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
                    stringBuilder.AppendLine($"*{packInfo.games[i].Description}*\n");

                }

                var message = await context.Channel.SendMessageAsync(stringBuilder.ToString());
                Destroyer.Message(message, DestroyTime.REALLYSLOW);
                

            }
        }

        //DO NOT ADD REQUIRES ATTRIBUTE OTHERWISE IT WILL SHOWUP IN THE HELP COMMAND
        [Command("packs")]
        [CoammandDescription("Displays an image containing all games in all packs",":frame_photo:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayPack(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE, true)) return;
            await context.Channel.SendMessageAsync("https://media.discordapp.net/attachments/1066085138791932005/1070771921643372564/image.png?width=674&height=902");
            
        }
    }
}
