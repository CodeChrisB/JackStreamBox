using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
        public async Task OpenGame(CommandContext context, int game)
        {
            await context.Channel.SendMessageAsync("Test");
            var task = JackStreamBoxUtility.OpenGame((Util.Data.Game)game-1);
            await task;
        }
    }
}
