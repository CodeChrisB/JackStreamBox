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
using JackStreamBox.Util.Data;
using System.Windows.Controls;
using DSharpPlus.EventArgs;
using System.Windows.Forms;
using System.Reflection.Metadata;

namespace JackStreamBox.Bot.Logic.Scheduled.Overwatch
{
    public class OverwatchVC
    {
        private static List<ulong> Streamer = new List<ulong>();    
        public static ulong guildId = 0;

        private static int cycleCount = 0;
        private static bool anyChange=false;
        public static async Task DistributeXP()
        {
            try
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

                if (!anyChange) return;
                anyChange = false;

                Console.WriteLine("Gave XP to some people");

                cycleCount++;
                if (cycleCount >= 4)
                {
                    SendBackUp(guild);
                    cycleCount = 0;
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static async void SendBackUp(DiscordGuild guild)
        {

            DiscordChannel channel = guild.GetChannel(ChannelId.LogChannel);


            byte[] byteArray = Encoding.UTF8.GetBytes(XPStore.GetAsString());
            MemoryStream stream = new MemoryStream(byteArray);

            var msg = new DiscordMessageBuilder()
            .WithContent("XP Backup in CSV format")
            .AddFile("xpBackup.log", stream);

            await channel.SendMessageAsync(msg);
        }

        private static async void CheckChannel(DiscordChannel channel)
        {

            //Requiremnts 3 People
            if (channel.Users.Count < 3) return;
            //1 and only 1 Streaming
            
            List<DiscordMember> channelStreamer = new List<DiscordMember>();
            for (int i= 0; i < channel.Users.Count; i++)
            {
                if (Streamer.Contains(channel.Users[i].Id))
                {
                    channelStreamer.Add(channel.Users[i]);
                }
            }

            if (channelStreamer.Count == 1)
            {
                AddXP(channelStreamer.FirstOrDefault());
                anyChange = true;
            }
        }

        private static async void AddXP(DiscordMember? discordMember)
        {
            if (discordMember == null) return;
            var guild = await Bot.Client.GetGuildAsync(guildId);
            int xpAdded = XPStore.AddXp(discordMember.Id);

            if (guildId > 0)
            {
                DiscordChannel channel = guild.GetChannel(ChannelId.LogChannel);
                await channel.SendMessageAsync($"[{BotData.ReadData(BotVals.BOT_NAME,"TB1")}] Added XP to {discordMember.Username} : {xpAdded} --> XP Now : {XPStore.GetById(discordMember.Id)}");
            }
        }

        internal static Task VoiceStateUpdatedAsync(DiscordClient sender, VoiceStateUpdateEventArgs args)
        {
            if (args.After.User.IsBot) return Task.CompletedTask;
            if (args.After.IsSelfStream)
            {
                if (!Streamer.Contains(args.After.Member.Id))
                {
                    Streamer.Add(args.After.Member.Id);
                }
            }
            else
            {
                if(Streamer.Contains(args.After.Member.Id)) 
                {
                    Streamer.Remove(args.After.Member.Id);
                }
            }

            return Task.CompletedTask;
        }
    }
}
