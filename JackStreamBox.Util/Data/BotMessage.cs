using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Util.Data
{
    public class BotMessage
    {
        public static string StartingGamePack = "Starting the party pack.";
        public static string OpenedGamePack = "Party pack is opened.";
        public static string StartingGame = "Starting the game.";
        public static string GameOpend = "Game opened.";
        public static string StartingStream = "Starting the stream.";
        public static string AllFinished = "Everything done, have a nice game !";
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
