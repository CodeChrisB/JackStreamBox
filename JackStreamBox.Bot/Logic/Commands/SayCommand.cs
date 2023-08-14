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
        public async Task Tell(CommandContext context,string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);

            int indexOfBadWord = badWords
            .Select(badWord => message.IndexOf(badWord))
            .FirstOrDefault();

            if (indexOfBadWord == -1) {
                await context.Channel.SendMessageAsync(message);
            }
            else
            {
                await context.Channel.SendMessageAsync($"{context.Member.Username} tried to use a bad word. Word Id is : {((indexOfBadWord + 4)*(indexOfBadWord + 1))*(234+ indexOfBadWord * 2)}-e{indexOfBadWord}");
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
            "fuck"
        };
    }
}
