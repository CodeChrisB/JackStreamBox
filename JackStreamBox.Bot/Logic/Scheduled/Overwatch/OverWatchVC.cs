using DSharpPlus.Entities;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.VisualBasic.ApplicationServices;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Report;
using JackStreamBox.Bot.Logic.Config;

namespace JackStreamBox.Bot.Logic.Scheduled.Overwatch
{
    public class OverwatchVC
    {
        public static ulong guildId = 0;
        public static async Task DistributeXP()
        {

            DiscordGuild guild = await Bot.Client.GetGuildAsync(guildId);
            if (guild == null) return;

            var channels = await guild.GetChannelsAsync();
            foreach (var channel in channels)
            {
                if (channel.Type == ChannelType.Voice)
                {
                    CheckChannel(channel);
                }
            }
        }

        private static async void CheckChannel(DiscordChannel channel)
        {
            
            //Requiremnts 3 People
            //if (channel.Users.Count < 3) return;
            //1 and only 1 Streaming
            var streamers = channel.Users.Where(user => user.Presence.Activities.Any(a => a.ActivityType == ActivityType.Playing));

            //0 Streamers or more than 1
            if(streamers.Count() == 1)
            {
                AddXP(streamers.FirstOrDefault());
            } 
        }

        private static async void AddXP(DiscordMember? discordMember)
        {
            if (discordMember == null) return;
            ulong xpAdded = XPStore.AddXp(discordMember.Id);

            if (guildId > 0)
            {
                var guild = await Bot.Client.GetGuildAsync(guildId);
                DiscordChannel channel = guild.GetChannel(ChannelId.LogChannel);
                await channel.SendMessageAsync($"Added XP to {discordMember.Username} : {xpAdded} --> XP Now : {XPStore.GetById(discordMember.Id)}");
            }
        }
    }
}
