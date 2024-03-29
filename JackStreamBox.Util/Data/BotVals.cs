﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Util.Data
{
    public class BotVals
    {
        public static readonly string VOTE_TIMER = "vote";
        public static readonly string PICK_TIMER = "pick";
        public static readonly string REQUIRED_VOTES = "require";
        public static readonly string BOT_NAME = "name";
        public static readonly string GAMES_HOSTED = "gamesHosted";
        public static readonly string MESSAGES_SENT = "messages_sent";
        public static readonly string VOTE_TIMEOUT = "vote_timeout";
        public static readonly string XP_TIME = "xpTime";
        public static readonly string ALIVE_TIME = "aliveTime";
		public static readonly string XP_AMOUNT = "xpAmount";
        public static readonly string XP_RANDOM = "xpRandom";
        public static readonly string RAFFLEXP = "raffleXP";
        public static readonly string XP_MIN_VC_SIZE = "xpVC";

        public static string[] GetKeys()
        {
            return new string[] { 
                VOTE_TIMER, 
                PICK_TIMER, 
                REQUIRED_VOTES, 
                BOT_NAME,
                "m1",
                "m2",
                "m3",
                "m4",
                "m5",
                "m6",
                "screen",
                GAMES_HOSTED, 
                VOTE_TIMEOUT, 
                XP_TIME,
                XP_AMOUNT,
                XP_RANDOM, 
                RAFFLEXP, 
                XP_MIN_VC_SIZE,
                ALIVE_TIME
            };
        }

    }
}
