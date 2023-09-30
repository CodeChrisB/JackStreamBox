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

        public static int JackBotCount { get; private set; }
        public static ulong[] JackBotPlayers { get; private set; } = new ulong[1];
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
                if (cycleCount >= 10)
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
            if (channel.Users.Count < BotData.ReadData(BotVals.XP_MIN_VC_SIZE, 3)) return;
            //1 and only 1 Streaming
            
            List<DiscordMember> channelStreamer  = channel.Users
                .Where(user => Streamer.Contains(user.Id))
                .ToList();

            if (channelStreamer.Count != 1) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[{BotData.ReadData(BotVals.BOT_NAME, "TB1")}]");
            for (int i = 0; i < channel.Users.Count; i++)
            {
                bool streamer = false;
                DiscordMember member = channel.Users[i];
                ulong xp = 0;
                if (member.Roles.Where(role => role.Name.ToLower() == "noxp").Count() > 0) return;
                if (Streamer.Contains(member.Id))
                {
                    streamer = true;
                    xp = await XPStore.AddBothXP(member.Id);
                    sb.AppendLine($"Added **HostXP** to {member.Username} : {xp} --> XP Now : {XPStore.GetHostXPById(member.Id)}");
                }
                else
                {
                    xp = await XPStore.AddPlayXP(member.Id);
                }

                sb.AppendLine($"Added **PlayXP** to {member.Username} : {xp} --> XP Now : {XPStore.GetPlayXpById(member.Id)}");

            }
            anyChange = true;
        }

        internal static Task VoiceStateUpdatedAsync(DiscordClient sender, VoiceStateUpdateEventArgs args)
        {
            if (args.After.User.IsBot) return Task.CompletedTask;

                
            //Joined JackBot VC
            DiscordChannel channel = (args.After.Channel == null ? args.Before.Channel : args.After.Channel);
            bool leftVC = args.After.Channel == null;

            if (channel.Id == ChannelId.JackBotVC)
            {
                if(channel.Users.Where(user => user.Id ==  ChannelId.BotAccount).ToList().Any()) 
                {
                    //streamer is  in vc 
                    JackBotCount = channel.Users.Count-1;
                }else
                {
                    //streamer is not in vc
                    JackBotCount = channel.Users.Count;
                }

                JackBotPlayers = channel.Users.Select(user => user.Id).ToArray();
            }

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


        public static bool IsInBotVC(ulong id)
        {
            return JackBotPlayers.Contains(id);
        }
    }
}
