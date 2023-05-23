using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Config
{
    public class Destroyer
    {
        public static async void Message(DiscordMessage message,TimeSpan seconds)
        {
            Task.Delay(seconds).Wait();
            if (message == null) return;
            try { 
                await message.DeleteAsync();
            }catch (Exception ex) { }
        }
    }
}
