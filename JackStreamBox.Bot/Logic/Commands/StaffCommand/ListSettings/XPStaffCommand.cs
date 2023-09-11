﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Commands.UserCommands.XP;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Bot.Logic.Scheduled.Overwatch;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand.ListSettings
{
    internal class XPStaffCommand : BaseCommandModule
    {

        const string DELETE_TEXT = "For real delete all xp";

        [Command("xpDelete")]
        [CoammandDescription("Deletes XP for all users", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task delete(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Woah you sure you want to do this?");
            sb.AppendLine($"type **!xpDelete {DELETE_TEXT}**");

            await context.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("xpDelete")]
        public async Task actualDelete(CommandContext context, string confirmMessage)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            if(confirmMessage != DELETE_TEXT)
            {
                await delete(context);
                return;
            }

            XPStore.DELETEALLXP();

            await context.Channel.SendMessageAsync($"{context.User.Mention} just deleted all XP...");
        }


        [Command("xpRaffle")]
        [CoammandDescription("Deletes XP for all users", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task xpRaffle(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;


            Dictionary<string, int> all = XPStore.GetAll();

            List<string> allTickets = new List<string>();
            ulong xp = 0;
            int xpToTicketRatio = BotData.ReadData(BotVals.RAFFLEXP, 100);
            foreach (var kvp in all)
            {
                xp += (ulong)kvp.Value;
                int tickets = kvp.Value / xpToTicketRatio;
                for(int i= 0; i < tickets; i++)
                {
                    allTickets.Add(kvp.Key);
                }
            }

            int xpTime = BotData.ReadData(BotVals.XP_TIME, 15);
            int xpAmount = BotData.ReadData(BotVals.XP_AMOUNT, 50);

            double timesPerHour = 60/(double)xpTime;
            double xpPerHour = xpAmount * timesPerHour;
            int totalHosted = (int)(xp / xpPerHour);




            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"In total there are {allTickets.Count} tickets.");
            sb.AppendLine($"Which means we hosted about {totalHosted} hours this month");
            await context.Channel.SendMessageAsync( sb.ToString() );
            await Task.Delay(1500);


            Random random = new Random(DateTime.Now.Millisecond);
            int totalTickets = allTickets.Count;
            int winningTicket = random.Next(1, totalTickets + 1);

            ulong winnerId = ulong.Parse(allTickets.ElementAt(winningTicket));


            var user = await context.Guild.GetMemberAsync(winnerId);
            int userXp = XPStore.GetById(user.Id);
            string top = await XPCommandLogic.TopMessage(context.ToCustomContext());

             await PlainEmbed.CreateEmbed(context)
                .Title("The Winner is:")
                .DescriptionAddLine($"{user.Mention}")
                .DescriptionAddLine($"Total XP: {userXp}")
                .DescriptionAddLine($"Total Tickets : {(userXp / xpToTicketRatio)}")
                .DescriptionAddLine($"Total Hours : {(int)(userXp / xpPerHour)}")
                .DescriptionAddLine("But Let's also not forget our Top 5 XP hosts:")
                .DescriptionAddLine(top)
                .Color(DiscordColor.Gold)
                .Build();
        }

    }
}