using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands;
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
        [Description("Checks what permission level you have.")]
        [Requires(PermissionRole.ANYONE)]
        public async Task WhatLevelAmI(CommandContext context)
        {

            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            int grantedLevel = RoleToLevel(context.Member.Roles);
            string text= "";
            switch (grantedLevel)
            {
                case 0: text = "You can not use this bot. All acesss besides this command is removed from your permissions.:skull:"; break;
                case 1: text = $"**Level {grantedLevel} ({RoleName((PermissionRole)grantedLevel)})**: Besides of voting you don't have alot of permissions"; break;
                case 2: text = $"**Level {grantedLevel} ({RoleName((PermissionRole)grantedLevel)})**: Besides of voting you don't have alot of permissions"; break;
                case 3: text = $"**Level {grantedLevel} ({RoleName((PermissionRole)grantedLevel)})**: You can use !startvote to try to start a game vote"; break;
                case 4: text = $"**Level {grantedLevel} ({RoleName((PermissionRole)grantedLevel)})**: Start games; make pee breaks; let the bot rejoin and more"; break;
                case 5: text = $"**Level {grantedLevel} ({RoleName((PermissionRole)grantedLevel)})**: You can do anything :crown:"; break;
                default: text = "Level N/A: could not find your level"; break;
            }
            string memberName = context.Member.DisplayName;
            var message = await context.Channel.SendMessageAsync($"\n{memberName} \n{text}");
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            Destroyer.Message(message, DestroyTime.SLOW);
        }


        private static bool IsBotPaused = false;
        [Command("toggle")]
        [Description("Toggle the whole bot, used for debugging without needing to access the server.")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task ToggleBot(CommandContext context)
        {
            int level = RoleToLevel(context.Member.Roles);
            if (level < (int)PermissionRole.DEVELOPER) return;
            CommandLevel.IsBotPaused = !CommandLevel.IsBotPaused;
            string name = BotSetings.ReadData(BotVals.BOT_NAME, "TB1");
            await context.Channel.SendMessageAsync($"The bot [{name}] is {(IsBotPaused ? "paused" : "resumed")}.");
        }
        public static bool CanExecuteCommand(CommandContext context,PermissionRole permissionLevel)
        {
            if (CommandLevel.IsBotPaused) return false;

            int grantedLevel = RoleToLevel(context.Member.Roles);
            int requiredLevel = (int)permissionLevel;

            //Check if inside the Jackbot VC or if developer used the command
            if (context.Channel.Id.ToString() != "1105184748701229066" && grantedLevel < (int)PermissionRole.STAFF) return false;

            bool canExecute = grantedLevel >= requiredLevel;


            if(!canExecute)
            {
                context.Channel.SendMessageAsync("You can not execute this");
            }

            return canExecute;
        }


        public static int RoleToLevel(IEnumerable<DiscordRole> roles)
        {
            int level = (int)PermissionRole.ANYONE;
            bool hasNegativeRole = false;
            if (!roles.Any()) return level;

            foreach (DiscordRole role in roles)
            {
                switch (role.Name) {
                    //Trusted
                    case "Level 2":
                    case "Level 3":
                    case "Level 4":
                    case "Level 5":
                    case "Level 6":
                    case "Level 7":
                        level = Math.Max(level, (int)PermissionRole.TRUSTED); 
                        break;
                    //Highly
                    case "Top hosts":
                    case "Captain Server-Booster🌟🌟":
                    case "VIP Members":
                        level = Math.Max(level, (int)PermissionRole.HIGHLYTRUSTED); 
                        break;
                    //Staff
                    case "Jack":
                    case "Box":
                        level = Math.Max(level, (int)PermissionRole.STAFF); 
                        break;
                    //Developer
                    case "Developer": 
                        level = Math.Max(level, (int)PermissionRole.DEVELOPER); 
                        break;
                    //Negative Roles
                    case "Quitter":
                    case "NoBot":
                        hasNegativeRole = true;
                       break;

                }
            }

            //todo negative roles
            if(hasNegativeRole) level  = (int)PermissionRole.NOBOT;
            //Rage Quitter
            //NoBot
            return level;
        }

        public static string RoleName(PermissionRole role)
        {
            switch (role)
            {
                case PermissionRole.ANYONE: return "Open";
                case PermissionRole.TRUSTED: return "Trust";
                case PermissionRole.HIGHLYTRUSTED: return "High";
                case PermissionRole.STAFF: return "Staff";
                case PermissionRole.DEVELOPER: return "Devs";
                default: return "Banned";
            }
        }
    }
}
