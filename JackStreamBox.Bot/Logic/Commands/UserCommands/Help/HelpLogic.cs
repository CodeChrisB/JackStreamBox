﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Help
{
    internal class HelpLogic
    {
        internal static async void DisplayHelp(CustomContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = "**How to use the bot?**",
                Description = ""
            };


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            string[] lines =
            {
                "The bot is using a voting system. Which consists of 3 main phases.",
                "**1. Vote Phase**",
                "After a game or when there is no game players can use **!vote X**",
                "After the first person voted there is a timelimit 60s in which all players should vote. 4 Votes are required to progress to the next phase.",
                "**Example usage**",
                "- `!vote 1` for pack 1",
                "- `!vote 2` for pack 2 a(works for all packs)",
                "Or shortcuts: `!1` for pack 1, `!2` for pack 2, etc.",
                "\n**2. Game Pick Phase**",
                "We now have picked the pack we will play",
                "The bot will create a message with 5 reactions \n:one: :two: :three: :four: :five:",
                "In the message the bot will explain which number is which game.",
                "Pick a game or multiple games you want to play, after the time is up the winner will be started.",
                "\n**3. Game Phase**",
                "The bot will show a message with the current progress of opening the game.",
                "Then you can play the game, after the game use !vote to start another game.",
                "\n**!commands** lets you view all commands",
                "\n:tools: Created by CCB for your joy! :computer:",
            };

            foreach (string line in lines)
            {
                sb.AppendLine(line);
            }


            helpEmbed.Description = sb.ToString();
            helpEmbed.ImageUrl = "https://media.discordapp.net/attachments/1066085138791932005/1135296119610552350/7u88ip.png";
            var message = await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            Destroyer.Message(message, DestroyTime.ULTRASLOW);
        }

        internal static async void DisplayCommands(CustomContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, true);
            Destroyer.Message(context.Message, DestroyTime.NORMAL);
        }

        private static async Task DisplayPackWithDescription(CustomContext context, bool appendDescription)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            CommandInfo[] ci = BotCommand.GetUserCommands();
            CommandEmbed.Show(context, "Help Page™", ci, appendDescription);
        }

        public static async Task SetRules(CustomContext context,string text)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            BotData.WriteCustomData<string>(BotData.RULE_FILE, text);
        }

        internal static void ShowRules(CustomContext context)
        {
            string rules = BotData.ReadCustomData<string>(BotData.RULE_FILE);
            PlainEmbed.CreateEmbed(context)
                .Title("Rules")
                .Description(rules)
                .BuildNDestroy(DestroyTime.SLOW);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
        }
    }
}
