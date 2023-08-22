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

        private const ulong DailyQuestionChannel = 1114225698056445992;

            

        [Command("daily")]
        [Description("Show staff commands")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task DailyQuestion(CommandContext context)
        {
            QuestionEmbed.Create(context, "Daily Question",
                new string[] { "Life is perfect", "Life is good", "It's okay", "I dont want to Jack about it" },
                new string[] {"heart","thumbsup","slight_smile","speak_no_evil"},
                DailyQuestionChannel
                );
        }

        [Command("poll")]
        [Description("!poll [Question] [Answer1] [Answer2],[Answer3] [Anser4]    (Atleast 2 Answers Max 4 Questions)")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task DailyQuestion4(CommandContext context,string question, string a1, string a2,string a3, string a4)
        {
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            QuestionEmbed.Create(context, question,
                new string[] { a1,a2,a3,a4 },
                new string[] { "green_circle", "red_circle", "orange_circle", "blue_circle" },
                context.Channel.Id
                );
        }

        [Command("poll")]
        public async Task DailyQuestion3(CommandContext context, string question, string a1, string a2, string a3)
        {

            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            QuestionEmbed.Create(context, question,
                new string[] { a1, a2, a3},
                new string[] { "green_circle", "red_circle", "orange_circle" },
                context.Channel.Id
                );
        }

        [Command("poll")]
        public async Task DailyQuestion2(CommandContext context, string question, string a1, string a2)
        {
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            QuestionEmbed.Create(context, question,
                new string[] { a1, a2 },
                new string[] { "green_circle", "red_circle" },
                context.Channel.Id
                );
        }


    }
}
