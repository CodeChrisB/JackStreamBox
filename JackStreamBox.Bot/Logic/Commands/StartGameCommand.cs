using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands
{
    public class StartGameCommand : BaseCommandModule
    {
        [Command("start")]
        [Description("Opens any game you want. Uses the game position.\nMad Verse city is the 3rd game in the 5th pack it's position is (5*5+3=28)\n  !closes any game that currently is held! Execution requires level 3")]
        public async Task OpenGame(CommandContext context, int game)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await context.Channel.SendMessageAsync("Test");
            var task = JackStreamBoxUtility.OpenGame((Util.Data.Game)game-1);
            await task;
        }
    }
}
