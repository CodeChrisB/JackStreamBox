using DSharpPlus.Entities;
using DSharpPlus;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Voting;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Menu
{
    internal class MenuLogic
    {
        internal static async void OpenMenu(CustomContext context)
        {

            if (VoteLogic.IsLocked())
            {
                VoteLogic.SendLockOutMessage(context);
                return;
            }
            var builder = new DiscordMessageBuilder()
            .WithContent("Hey everyone ! Click on a pack you want to play !")
            .AddComponents(new DiscordComponent[]
            {
               Btn(ButtonId.PACK1,"Pack  1"),
               Btn(ButtonId.PACK2,"Pack  2"),
               Btn(ButtonId.PACK3,"Pack  3"),
               Btn(ButtonId.PACK4,"Pack  4"),
               Btn(ButtonId.PACK5,"Pack  5"),

            });

            builder.AddComponents(new DiscordComponent[]
            {
               Btn(ButtonId.PACK6,"Pack 6 "),
               Btn(ButtonId.PACK7,"Pack 7 "),
               Btn(ButtonId.PACK8,"Pack 8 "),
               Btn(ButtonId.PACK9,"Pack 9 "),
               Btn(ButtonId.PACK10,"Pack 10"),
            });


            await context.Channel.SendMessageAsync(builder);
        }


        private static DiscordButtonComponent Btn(string id,string text)
        {
            return new DiscordButtonComponent(ButtonStyle.Secondary, id, text, false);
        }
    }
}
