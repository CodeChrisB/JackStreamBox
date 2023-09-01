using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace JackStreamBox.Bot.Logic.Commands.ScheduledCommands
{
    internal class DailyQuestionCommand : BaseCommandModule
    {
        string[] reactions = new string[] { "green_circle", "red_circle", "blue_circle", "yellow_circle", "orange_circle", "red_circle" };
        string[] daily = new string[] { "heart", "thumbsup", "slight_smile", "speak_no_evil" };
        private async Task CreateQuestion(CommandContext context, string question, string[] answers, string[] reactions,string? url) =>
            QuestionEmbed.Create(context, question, answers,reactions.Take(answers.Length).ToArray(), context.Channel.Id,url);

        [Command("daily")]
        [Description("Show staff commands")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task DailyQuestion(CommandContext context)
        {
            await CreateQuestion(context, "Daily Question", new string[] { "Life is perfect", "Life is good", "It's okay", "I dont want to Jack about it" },daily,null);
        }

        [Command("poll")]
        [Description("!poll [Question] [Answer1] [Answer2] [Answer3] [Answer4] [Answer5] [Answer6]    (At least 1 Answer Max 6 Answers)")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task DailyQuestion(CommandContext context, string question, string answer1 = null, string answer2 = null, string answer3 = null, string answer4 = null, string answer5 = null, string answer6 = null)
        {
            string[] answers = new string[] { answer1, answer2, answer3, answer4, answer5, answer6 }
                .Where(answer => !string.IsNullOrEmpty(answer))
                .ToArray();

            if (answers.Length < 1 || answers.Length > 6)
            {
                // Handle invalid number of answers
                return;
            }

            await CreateQuestion(context, question, answers,reactions,null);
        }

        [Command("poll+")]
        [Description("!poll [ImageUrl] [Question] [Answer1] [Answer2] [Answer3] [Answer4] [Answer5] [Answer6]  (At least 1 Answer Max 6 Answers)")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task DailyQuestion(CommandContext context, string url, string question, string answer1 = null, string answer2 = null, string answer3 = null, string answer4 = null, string answer5 = null, string answer6 = null)
        {
            string[] answers = new string[] { answer1, answer2, answer3, answer4, answer5, answer6 }
                .Where(answer => !string.IsNullOrEmpty(answer))
                .ToArray();

            if (answers.Length < 1 || answers.Length > 6)
            {
                // Handle invalid number of answers
                return;
            }

            await CreateQuestion(context, question, answers, reactions, url);
        }
    }
}