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
        [Description("Use the bot to speak")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task Tell(CommandContext context,string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            await context.Channel.SendMessageAsync(message);
        }
    }
}
