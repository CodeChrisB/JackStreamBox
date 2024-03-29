﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder
{
    internal class CommandEmbed
    {
        internal static async void Show(CustomContext context, string title, CommandInfo[] ci, bool appendDescription)
        {
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = "If u see this message something went wrong"
            };


            StringBuilder sb = new StringBuilder();
            int level = CommandLevel.RoleToLevel(context.Member.Roles);
            int currentlevel = -1;
            sb.Append("Here are all commands you can use");
            foreach (var ciItem in ci)
            {
                if (level >= (int)ciItem.Role)
                {
                    if (currentlevel < (int)ciItem.Role)
                    {
                        sb.AppendLine($"\n**Level {(int)ciItem.Role} - {CommandLevel.RoleName(ciItem.Role)}**");
                        currentlevel = (int)ciItem.Role;
                    }

                    if (appendDescription)
                    {
                        sb.AppendLine($"- {ciItem.Emoji} **!{ciItem.Name}** {ciItem.Description}");
                    }else
                    {
                        sb.AppendLine($"**!{ciItem.Name}**");
                    }
                }
            }
            //sb.AppendLine($"**=========╣ End of commands ╠=========**");

            helpEmbed.Description = sb.ToString();

            var embed = await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
            Destroyer.Message(embed, DestroyTime.REALLYSLOW);
            
        }
    }
}
