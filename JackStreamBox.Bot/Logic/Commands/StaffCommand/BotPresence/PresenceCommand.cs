﻿using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand.BotPresence
{
    internal class PresenceCommand
    {
        public static void SetDefaultActivty()
        {
            DiscordActivity activity = new DiscordActivity();
            
        }
        
        //It is actual possible to set the rich presence of the bot
        //this is cool i want cool things
        //but also i want to play games
        //so i will do this later
        //
        /*
         
         static void UpdatePresence()
        {
            DiscordRichPresence discordPresence;
            memset(&discordPresence, 0, sizeof(discordPresence));
            discordPresence.state = "Playing Solo";
            discordPresence.details = "Competitive";
            discordPresence.startTimestamp = 1507665886;
            discordPresence.endTimestamp = 1507665886;
            discordPresence.largeImageText = "Numbani";
            discordPresence.smallImageText = "Rogue - Level 100";
            discordPresence.partyId = "ae488379-351d-4a4f-ad32-2b9b01c91657";
            discordPresence.partySize = 1;
            discordPresence.partyMax = 5;
            discordPresence.joinSecret = "MTI4NzM0OjFpMmhuZToxMjMxMjM= ";
            Discord_UpdatePresence(&discordPresence);
        }
         */
    }
}
