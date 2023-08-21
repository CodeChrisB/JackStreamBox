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

namespace JackStreamBox.Bot.Logic.Commands
{
    public class SayCommand : BaseCommandModule
    {
        [Command("say")]
        [Description("Use the bot to speak.")]
        [Requires(PermissionRole.STAFF)]
        public async Task Tell(CommandContext context, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);

            bool containsBadWord = badWords.Any(badWord => message.Contains(badWord));

            if (!containsBadWord) {
                await context.Channel.SendMessageAsync(String.Join(" ",message));
            }
            else
            {
                var logChannel = await context.Client.GetChannelAsync(1114225698056445992);

                string username = context.Member.Nickname;
                await logChannel.SendMessageAsync($"{username}\n: Wanted to say {message} using the bot !");
            }
        }

        private string[] badWords = new string[]
        {
            "test-word-so-that-gray-wont-ban-me",
            "niger",
            "nigger",
            "nigga",
            "niga",
            "pussy",
            "cunt",
            "whore",
            "fuck",
        };
    }
}
