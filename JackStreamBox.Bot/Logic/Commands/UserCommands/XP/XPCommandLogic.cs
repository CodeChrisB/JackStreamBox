﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Scheduled.Overwatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.XP
{
    internal class XPCommandLogic
    {

        public static async void ShowTopXP(InteractionContext ctx)
        {
            var builder = PlainEmbed.CreateEmbed(ctx.ToCustomContext());

            builder.Title("📋 Guild Host XP Leaderboards");
            builder.DescriptionAddLine("**TOP 5 HOST**");
            builder.Color(DiscordColor.HotPink);

            string top = await TopMessage(ctx.ToCustomContext());
            builder.DescriptionAddLine(top);
            await builder.Build();

        }


        public static async Task<string> TopMessage(CustomContext context)
        {
            Dictionary<string, int> list = XPStore.GetTop(5);
            int i = 1;
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in list)
            {
                var user = await context.Guild.GetMemberAsync(ulong.Parse(kvp.Key));

                sb.AppendLine($"#{i}| {user.Mention} XP: **{kvp.Value}**");
            }

            return sb.ToString();   
        }

        public static async void ShowOwnXP(InteractionContext ctx)
        {
            int xp = XPStore.GetById(ctx.User.Id);
            int pos = XPStore.GetPosById(ctx.User.Id);
            await PlainEmbed.CreateEmbed(ctx.ToCustomContext())
                .Title($"Host XP of {ctx.User.Username}")
                .Description($"#{pos + 1}|{ctx.User.Mention} XP: **{xp}**")
                .Build();
        }
    }
}
