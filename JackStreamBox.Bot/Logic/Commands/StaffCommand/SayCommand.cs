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
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using DSharpPlus.SlashCommands;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    public class SayCommand : BaseCommandModule
    {
        [Command("say")]
        [CoammandDescription("Use the bot to speak. You can even use line breaks and mentions people!!",":speech_ballon:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Tell(CommandContext context, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);


            if (context.Message.Reference != null)
            {
                await context.Message.Reference.Message.RespondAsync(message);
            }else
            {
                await context.Channel.SendMessageAsync(message);
            }
            SendLogMessage(context,message);
        }

        [Command("troll")]
        [CoammandDescription("Use the bot to speak. You can even use line breaks and mentions people!!", ":nerd:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Troll(CommandContext context, [RemainingText] string message)
        {

                message = GetTrollMessage(context, message);

                if(message == "")
                {
                    Destroyer.Message(context.Message,DestroyTime.INSTANT);
                }
                char[] result = new char[message.Length];
                int state = 0;
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] == ' ')
                    {
                        result[i] = message[i];
                    }
                    else if (state % 2 == 0)
                    {
                        result[i] = Char.ToUpper(message[i]);
                        state++;
                    }
                    else
                    {
                        result[i] = Char.ToLower(message[i]);
                        state++;
                    }
                }

            await Tell(context,new string(result));
        }

        public string GetTrollMessage(CommandContext context,string message)
        {
            if (message != null && message.Length > 0) return message;

            if (context.Message.Reference != null)
            {
                return context.Message.Reference.Message.Content;
            }
            return "";
        }
        [Command("embed")]
        [CoammandDescription("Use the bot to speak. Title in \"Qutation Marks\" Message without them, can even use line breaks for the message.",":newspaper:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Embed(CommandContext context, string title, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);



            DiscordEmbedBuilder embedBuilder = PlainEmbed
                .CreateEmbed()
                .Title(title)
                .Description(message)
                .Color(DiscordColor.Green)
                .GetBuilder();


            await context.Channel.SendMessageAsync(embed: embedBuilder.Build()).ConfigureAwait(false);
            SendLogEmbed(context,embedBuilder);
        }

        [Command("embed+")]
        [CoammandDescription("!embed+ \"[Image Url]\" \"[Title]\"  [Message]", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Embed(CommandContext context, string url, string title, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);

            DiscordEmbedBuilder embed = PlainEmbed
                .CreateEmbed(context)
                .ImageUrl(url)
                .Title(title)
                .Description(message)
                .GetBuilder();

                
            SendLogEmbed(context, embed);
            PlainEmbed.BuildNDestroy(context,embed,DestroyTime.SLOW);
        }

        private async void SendLogEmbed(CommandContext context, DiscordEmbedBuilder embedBuilder)
        {
            var LogChannel = await context.Client.GetChannelAsync(ChannelId.LogChannel);

            embedBuilder.Description = $"[{context.Member.Username}]\n {embedBuilder.Description}";
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
                sb.AppendLine($"{context.Member.Username} reply to {context.Message.Reference.Message.Author.Username}");
                sb.AppendLine($"Replied to : {context.Message.Reference.Message.Content}");
            }
            else
            {
                //Just send message
                sb.AppendLine($"Sent by : {context.Member.Username}");
            }


            sb.AppendLine($"Message : {message}");
            return sb.ToString();
        }
    }
}

