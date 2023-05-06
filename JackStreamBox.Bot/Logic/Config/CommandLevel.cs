using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Config
{
    public class CommandLevel : BaseCommandModule
    {
        [Command("level")]
        [Description("Checks what permission level you have. Execution requires level 0.")]
        public async Task WhatLevelAmI(CommandContext context)
        {
            int grantedLevel = RoleToLevel(context.Member.Roles);
            string message= "";
            switch (grantedLevel)
            {
                case 0: message = "Level 0: Besides of voting you don't have alot of permissions"; break;
                case 1: message = "Level 1: Besides of voting you don't have alot of permissions"; break;
                case 2: message = "Level 2: Besides of voting you currently don't have alot of permissions"; break;
                case 3: message = "Level 3: You can use !startvote if the bot breaks and does not stream anymore"; break;
                case 4: message = "Level 4: You can do anything :crown:"; break;
                default: message = "Level 0: Besides of voting you don't have alot of permissions"; break;
            }
            string memberName = context.Member.DisplayName;
            await context.Channel.SendMessageAsync($"{memberName} - {message}");
        }

        public static bool CanExecuteCommand(CommandContext context,int permissionLevel)
        {
            int grantedLevel = RoleToLevel(context.Member.Roles);
            bool canExecute = grantedLevel >= permissionLevel;
            if(!canExecute)
            {
                context.Channel.SendMessageAsync("You can not execute this");
            }
            return canExecute;
        }


        private static int RoleToLevel(IEnumerable<DiscordRole> roles)
        {
            int level = PermissionRole.ANYONE;
            if (!roles.Any()) return level;

            foreach (DiscordRole role in roles)
            {
                switch (role.Name) {
                    case "Top Host": level = Math.Max(level,PermissionRole.HIGHLYTRUSTED); break;
                    case "Developer": level = Math.Max(level, PermissionRole.DEVELOPER); break;
                }
            }

            return level;
        }
    }
}
