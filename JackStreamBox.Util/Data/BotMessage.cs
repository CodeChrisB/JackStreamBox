using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Util.Data
{
    public class BotMessage
    {
        public static string StartingGamePack = "Open Pack";
        public static string OpenedGamePack = "Pack Is Openend";
        public static string StartingGame = "Picking th game";
        public static string GameOpend = "Game is started";
        public static string StartingStream = "Starting the stream.";
        public static string AllFinished = "Found a bug? use **!report [Your Message]**";
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
