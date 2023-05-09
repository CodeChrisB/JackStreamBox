﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
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
        [Requires(PermissionRole.NOBOT)]
        public async Task WhatLevelAmI(CommandContext context)
        {

            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.NOBOT)) return;

            int grantedLevel = RoleToLevel(context.Member.Roles);
            string message= "";
            switch (grantedLevel)
            {
                case 0: message = "You can not use this bot. All acesss besides this command is removed from your permissions.:skull:"; break;
                case 1: message = "Level 0: Besides of voting you don't have alot of permissions"; break;
                case 2: message = "Level 1: Besides of voting you don't have alot of permissions"; break;
                case 3: message = "Level 2: You can use !startvote"; break;
                case 4: message = "Level 3: You can use !startvote"; break;
                case 5: message = "Level 4: You can do anything :crown:"; break;
                default: message = "Level 0: Besides of voting you don't have alot of permissions"; break;
            }
            string memberName = context.Member.DisplayName;
            await context.Channel.SendMessageAsync($"{memberName} \n{message}");
        }

        public static bool CanExecuteCommand(CommandContext context,PermissionRole permissionLevel)
        {
            int grantedLevel = RoleToLevel(context.Member.Roles);
            int requiredLevel = (int)permissionLevel;

            //Currently only developers can use the bot
            //if(grantedLevel < (int)PermissionRole.DEVELOPER) return false;


            //Check if inside the Jackbot VC
            if (context.Channel.Id.ToString() != "1105184748701229066") return false;

            bool canExecute = grantedLevel >= requiredLevel;



            if(!canExecute)
            {
                context.Channel.SendMessageAsync("You can not execute this");
            }


            return canExecute;
        }


        private static int RoleToLevel(IEnumerable<DiscordRole> roles)
        {
            int level = (int)PermissionRole.ANYONE;
            bool hasNegativeRole = false;
            if (!roles.Any()) return level;

            foreach (DiscordRole role in roles)
            {
                switch (role.Name) {
                    //Trusted
                        //idk not used atm
                    //Highly
                    case "Top hosts": level = Math.Max(level,(int)PermissionRole.HIGHLYTRUSTED); break;
                    case "Captain Server-Booster🌟🌟": level = Math.Max(level, (int)PermissionRole.HIGHLYTRUSTED); break;
                    //Staff
                    case "Jack": level = Math.Max(level, (int)PermissionRole.STAFF); break;
                    //Developer
                    case "Developer": level = Math.Max(level, (int)PermissionRole.DEVELOPER); break;
                    //Negative Roles
                    case "Quitter": hasNegativeRole = true;break;

                }
            }

            //todo negative roles
            if(hasNegativeRole) level  = (int)PermissionRole.NOBOT;
            //Rage Quitter
            //NoBot
            return level;
        }
    }
}
