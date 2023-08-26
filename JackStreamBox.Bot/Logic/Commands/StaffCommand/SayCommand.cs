using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSharpPlus.Entities;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    public class SayCommand : BaseCommandModule
    {
        [Command("say")]
        [Description("Use the bot to speak. You can even use line breaks !")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Tell(CommandContext context, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);


            await context.Channel.SendMessageAsync(message);
            SendLogMessage(context,message);
        }

        [Command("embed")]
        [Description("Use the bot to speak. Title in \"Qutation Marks\" Message without them, can even use line breaks for message")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Embed(CommandContext context, string title, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);



            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.Title = title;
            embedBuilder.Description = message;
            embedBuilder.Color = DiscordColor.Green;

            await context.Channel.SendMessageAsync(embed: embedBuilder.Build()).ConfigureAwait(false);
            SendLogEmbed(context,embedBuilder);
        }

        private async void SendLogEmbed(CommandContext context, DiscordEmbedBuilder embedBuilder)
        {
            var LogChannel = await context.Client.GetChannelAsync(ChannelId.LogChannel);

            embedBuilder.Description = $"[{context.Member.Nickname}] {embedBuilder.Description}";
            await LogChannel.SendMessageAsync(embed: embedBuilder.Build()).ConfigureAwait(false);
        }

        private async void SendLogMessage(CommandContext context, string message)
        {
            var LogChannel = await context.Client.GetChannelAsync(ChannelId.LogChannel);
            await LogChannel.SendMessageAsync(await LogMessage(context, message));
        }
        private async Task<string> LogMessage(CommandContext context,string message)
        {
            StringBuilder sb = new StringBuilder();

            if (context.Message.Reference != null)
            {
                //Reply to referenced Message
                await context.Message.Reference.Message.RespondAsync(message);
                sb.AppendLine($"{context.Member.Nickname} reply to {context.Message.Reference.Message.Author.Username}");
                sb.AppendLine($"Replied to : {context.Message.Reference.Message.Content}");
            }
            else
            {
                //Just send message
                sb.AppendLine($"Sent by : {context.Member.Nickname}");
            }


            sb.AppendLine($"Message : {message}");
            return sb.ToString();
        }
    }
}

