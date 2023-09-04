using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands._Helper.ChartBuilder;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using JackStreamBox.Util.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Voting
{
    internal class VoteCommand : BaseCommandModule
    {



        [Command("vote")]
        [CoammandDescription($"Vote for the pack/category you want to play, when 4 players vote one of the voted categories will be picked. ", ":ballot_box:")]
        [Requires(PermissionRole.ANYONE)]
        public static async Task VoteX(CommandContext context, string voteCategory)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            VoteLogic.Vote(context.ToCustomContext(), voteCategory);
        }

        //Give help when players to stupid to vote
        [Command("vote")]
        public async Task vote(CommandContext context)
        {
            PackInfo.VoteCategories(context.ToCustomContext());
        }



        // Commands for using only numbers to vote

        [Command("1")] public async Task Vote1(CommandContext context) => await VoteX(context, "1");
        [Command("2")] public async Task Vote2(CommandContext context) => await VoteX(context, "2");
        [Command("3")] public async Task Vote3(CommandContext context) => await VoteX(context, "3");
        [Command("4")] public async Task Vote4(CommandContext context) => await VoteX(context, "4");
        [Command("5")] public async Task Vote5(CommandContext context) => await VoteX(context, "5");
        [Command("6")] public async Task Vote6(CommandContext context) => await VoteX(context, "6");
        [Command("7")] public async Task Vote7(CommandContext context) => await VoteX(context, "7");
        [Command("8")] public async Task Vote8(CommandContext context) => await VoteX(context, "8");
        [Command("9")] public async Task Vote9(CommandContext context) => await VoteX(context, "9");
        [Command("10")] public async Task Vote10(CommandContext context) => await VoteX(context, "10");





    }
}
