using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand.ListSettings
{
    internal class RuleCommands : BaseCommandModule
    {

        [Command("rule+")]
        [CoammandDescription("Adds a rule", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Add(CommandContext context, string url)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ListLogic.Add(context, ListSerializer.RULE, url);
        }

        [Command("rule-")]
        [CoammandDescription("Removes a rule with the indicated index", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]

        public async Task Remove(CommandContext context, int index)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ListLogic.Remove(context, ListSerializer.RULE, index);
        }

        [Command("rule")]
        [CoammandDescription("Show all rules with their indexes needed to remove them", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task GetAll(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ListLogic.GetAll(context, ListSerializer.RULE);
        }
    }
}
