using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands.StaffCommand.ListSettings;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Menu
{
    internal class MenuCommand : BaseCommandModule
    {
        [Command("menu")]
        [CoammandDescription("Opens a menu that let's you vote and do other stuff", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task GetAll(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            MenuLogic.OpenMenu(context.ToCustomContext());
        }
    }
}
