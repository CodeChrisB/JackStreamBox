using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands
{
    public class StartGame : BaseCommandModule
    {
        [Command("start")]
        public async Task OpenGame(CommandContext context)
        {
            await context.Channel.SendMessageAsync("Test");
            var task = JackStreamBoxUtility.OpenGame(Util.Data.Game.Jobjob);
            await task;
        }
    }
}
