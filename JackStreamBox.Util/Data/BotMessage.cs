using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Util.Data
{
    public class BotMessage
    {
        public static string StartingGamePack = BotData.ReadData("m1", "Open Pack");
        public static string OpenedGamePack = BotData.ReadData("m2", "Pack Is Openend");
        public static string StartingGame = BotData.ReadData("m3", "Picking th game");
        public static string GameOpend = BotData.ReadData("m4", "Game is started");
        public static string StartingStream = BotData.ReadData("m5", "Starting the stream.");
        public static string AllFinished = BotData.ReadData("m6", "Found a bug? use **!report [Your Message]**");

        public static void ReloadMessages()
        {
            StartingGamePack = BotData.ReadData("m1", "Open Pack");
            OpenedGamePack = BotData.ReadData("m2", "Pack Is Openend");
            StartingGame = BotData.ReadData("m3", "Picking th game");
            GameOpend = BotData.ReadData("m4", "Game is started");
            StartingStream = BotData.ReadData("m5", "Starting the stream.");
            AllFinished = BotData.ReadData("m6", "Found a bug? use **!report [Your Message]**");
        }
    }



    public enum VoteStatus
    {
        OnStartingGamePack,
        OnOpendGamePack,
        OnStartingGame,
        OnGameOpend,
        OnStartingStream,
        OnAllFinished
    }
}
