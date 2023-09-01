using DSharpPlus.CommandsNext;
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
    internal class QuestionEmbed
    {
        internal static async void Create(CommandContext context, string title, string[] answers, string[]emojis,ulong ChannelId,string? url)
        {
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            if (answers.Length != emojis.Length) return;

            if (title.Length > 255)
            {
                title = title.Substring(0, 250);
                title += "...";
            }
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = "If u see this message something went wrong"
            };

            if(url != null)
            {
                embed.ImageUrl = url;
            }


            StringBuilder sb = new StringBuilder();
            DiscordEmoji[] dmojis = CreateEmojis(context, emojis);


            for (int i = 0; i < answers.Length; i++)
            {
                sb.AppendLine($"{dmojis[i].Name} - {answers[i]}");
            }

            embed.Description = sb.ToString();


            var logChannel = await context.Client.GetChannelAsync(ChannelId);
            var message = await logChannel.SendMessageAsync(embed: embed).ConfigureAwait(false);
    

            for (int i = 0; i < emojis.Length; i++)
            {
                await message.CreateReactionAsync(dmojis[i]).ConfigureAwait(false);
            }
        }

        private static DiscordEmoji[] CreateEmojis(CommandContext context, string[] name)
        {
            List<DiscordEmoji> emojis = new List<DiscordEmoji>();

            foreach (string emoji in name) {
                emojis.Add(DiscordEmoji.FromName(context.Client, $":{emoji}:"));
            }
            return emojis.ToArray();
        }
    }
}
