using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Config
{
    public class CommandLevel : BaseCommandModule
    {
        private static bool IsBotPaused = false;
        public static bool IsDevBot = false;
        public static string BotName = "xxxx";


        [Command("level")]
        [CoammandDescription("Checks what permission level you have.",":straight_ruler:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task WhatLevelAmI(CommandContext context)
        {

            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            int grantedLevel = RoleToLevel(context.Member.Roles);
   
            FluentBuilder builder = PlainEmbed.CreateEmbed(context).Title($"Level of {context.Member.DisplayName}");

            if(grantedLevel == (int)PermissionRole.NOBOT)
            {
                builder
                    .DescriptionAddLine("You can not use this bot. All acesss besides this command is removed from your permissions.:skull:")
                    .Color(DiscordColor.DarkRed);
            }
            else
            {
                if (grantedLevel >= (int)PermissionRole.ANYONE)
                {
                    builder
                        .DescriptionAddLine("- Voting rights")
                        .Color(DiscordColor.Yellow);

                }
                if (grantedLevel >= (int)PermissionRole.TRUSTED)
                {
                    builder
                        .DescriptionAddLine("- Report rights")
                        .Color(DiscordColor.Green);
                }
                if (grantedLevel >= (int)PermissionRole.HIGHLYTRUSTED)
                {
                    builder
                         .DescriptionAddLine("- Report rights")
                         .DescriptionAddLine("- Pause games for toilet breaks")
                         .DescriptionAddLine("- Restart the bot")
                         .Color(DiscordColor.Blue);
                }
                if(grantedLevel >= (int)PermissionRole.STAFF)
                {
                    builder
                        .DescriptionAddLine("- Moderation via Bot")
                        .DescriptionAddLine("- Embed creator")
                        .DescriptionAddLine("- Poll creator")
                        .Color(DiscordColor.Gold);
                        
                }
                if(grantedLevel >= (int)PermissionRole.DEVELOPER)
                {
                    builder
                        .DescriptionAddLine("- Get logs")
                        .DescriptionAddLine("- Update the bot")
                        .Color(DiscordColor.Purple);
                }  
            }

            await builder.BuildNDestroy(DestroyTime.SLOW);
        
        }



        [Command("toggle")]
        [CoammandDescription("Toggle the whole bot, used for debugging without needing to access the server.", ":flashlight:")]
        [ModCommand(PermissionRole.DEVELOPER)]
        public async Task ToggleBot(CommandContext context)
        {
            int level = RoleToLevel(context.Member.Roles);
            if (level < (int)PermissionRole.DEVELOPER) return;
            CommandLevel.IsBotPaused = !CommandLevel.IsBotPaused;
            string name = BotData.ReadData(BotVals.BOT_NAME, "TB1");
            await context.Channel.SendMessageAsync($"The bot [{name}] is {(IsBotPaused ? "paused" : "resumed")}."); 
        }

        [Command("toggle")]
        public async Task ToggleBotByName(CommandContext context, string botName)
        {
            //Only toggle if its the correct bot
            if(BotData.ReadData(BotVals.BOT_NAME, "TB1").ToLower() != botName.ToLower()) return;

            await ToggleBot(context);
        }


        
        public static bool CanExecuteCommand(InteractionContext context, PermissionRole permissionLevel, bool ignoreChannel = false)
        {
            return CanExecuteCommand(context.ToCustomContext(), permissionLevel, ignoreChannel);
        }
        public static bool CanExecuteCommand(CommandContext context,PermissionRole permissionLevel,bool ignoreChannel = false)
        {
            return CanExecuteCommand(context.ToCustomContext(), permissionLevel, ignoreChannel);
        }
        public static bool CanExecuteCommand(CustomContext context, PermissionRole permissionLevel, bool ignoreChannel = false)
        {
            if (CommandLevel.IsBotPaused) return false;
            int grantedLevel = RoleToLevel(context.Member.Roles);
            return InternalCanExecute(grantedLevel, permissionLevel, context.Channel.Id, ignoreChannel);
        }


        private static bool InternalCanExecute(int grantedLevel, PermissionRole permissionLevel, ulong  channelId,bool ignoreChannel)
        {
            int requiredLevel = (int)permissionLevel;

            //Check if inside the Jackbot VC or if staff used the command
            if (!ignoreChannel && channelId != ChannelId.JackBotVC && grantedLevel < (int)PermissionRole.STAFF) return false;

            //Check if inside dev channel only dev bot can execute stuff here
            if (channelId == ChannelId.DevChannel && IsDevBot == false) return false;

            bool canExecute = grantedLevel >= requiredLevel;

            return canExecute;
        }

        internal static bool IsBanned(CommandContext context)
        {
            return (int)PermissionRole.NOBOT == RoleToLevel(context.Member.Roles);
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
                case PermissionRole.ANYONE: return "Anyone";
                case PermissionRole.TRUSTED: return "Trused Users";
                case PermissionRole.HIGHLYTRUSTED: return "Top Hosts";
                case PermissionRole.STAFF: return "Staff Members";
                case PermissionRole.DEVELOPER: return "Developers";
                default: return "Banned";
            }
        }

    }
}
