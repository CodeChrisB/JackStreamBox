using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper
{
    internal class LogChannel
    {

        public static async Task LogPasteBin(CommandContext context, string text)
        {
            if (DocGenerator.PASTE_BIN_KEY.Length > 4)
            {
                var logChannel = await context.Client.GetChannelAsync(ChannelId.LogChannel);
                await logChannel.SendMessageAsync(text);
                Console.WriteLine("Log - sent Message to Log Channel");
            }
        }
    }
}
