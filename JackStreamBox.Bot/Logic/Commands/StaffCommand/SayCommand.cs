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

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    public class SayCommand : BaseCommandModule
    {
        [Command("say")]
        [Description("Use the bot to speak.")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Tell(CommandContext context, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);


            var LogChannel = await context.Client.GetChannelAsync(ChannelId.LogChannel);
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
                await context.Channel.SendMessageAsync(message);
                BotData.IncrementValue("message");
            }


            sb.AppendLine($"Message : {message}");
            //Log messages do not count to total messages sent
            await LogChannel.SendMessageAsync(sb.ToString());
        }
    }
}

