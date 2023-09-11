using DSharpPlus.Entities;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace JackStreamBox.Bot.Logic.Scheduled.Overwatch
{
    internal class XPStore
    {
        /* Public API */

        public static void LoadData()
        {
            LoadDataFromFile();
        }

        public static async Task<ulong> AddPlayXP(ulong id)
        {
            return await AddXP(id, false, true);
        }

        public static async Task<ulong> AddBothXP(ulong id)
        {
            return await AddXP(id, true, true);
        }

        private static async Task<ulong> AddXP(ulong id,bool hostXp,bool playXP)
        {

            Random random = new Random(DateTime.Now.Millisecond);
            ulong xpToAdd = (ulong)BotData.ReadData(BotVals.XP_AMOUNT, 10);
            xpToAdd += (ulong)random.Next(0, BotData.ReadData(BotVals.XP_RANDOM, 0));

            int index = PlayerStructList.FindIndex(player => player.Id == id);

            if (index < 0) return 0;

            Player playerForXP= PlayerStructList.Find(player => player.Id == id);


            if (hostXp) playerForXP.HostXP += xpToAdd;
            if (playXP) playerForXP.PlayXP += xpToAdd;

            //Just to be safe
            int newIndex = PlayerStructList.FindIndex(player => player.Id == id);
            PlayerStructList[newIndex] = playerForXP;

            return xpToAdd;


        }

        public static object GetPlayXpById(ulong id)
        {
            Player? player = PlayerStructList.FirstOrDefault(player => player.Id == id);
            if (player == null) return 0;

            if (player.HasValue)
            {
                return player.Value.PlayXP;
            }

            return 0;
        }

        public static ulong GetHostXPById(ulong id)
        {
            Player? player = PlayerStructList.FirstOrDefault(player => player.Id == id);
            if (player == null) return 0;

            if (player.HasValue)
            {
                return player.Value.HostXP;
            }

            return 0;
        }
        public static int GetPosById(ulong id)
        {
            return PlayerStructList.OrderByDescending(user => user.HostXP).ToList().FindIndex(pair => pair.Id == id);
        }

        public static List<Player> GetTop(int n)
        {
            //gets top n user by xp

            return PlayerStructList.OrderByDescending(kv => kv.HostXP).Take(n).ToList();

        }

        internal static void DELETEALLXP()
        {
            PlayerStructList = new List<Player>();
            SaveDataToFile(); // :(
        }

        /* End Of Public API */

        const string FileName = "xpSheet";
        private static List<Player> PlayerStructList = new List<Player>();
        private static void SaveDataToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    foreach (var player in PlayerStructList)
                    {
                        writer.WriteLine(player.ToString());
                    }
                    writer.Flush();
                }
            }catch (Exception e)
            {
                SaveDataToFile();
            }

        }

        private static void LoadDataFromFile()
        {
            PlayerStructList = new List<Player>();

            if (File.Exists(FileName))
            {
                string[] lines = File.ReadAllLines(FileName);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        var playerId = ulong.Parse(parts[0]);
                        var hostXP = ulong.Parse(parts[1]);
                        var playXP = ulong.Parse(parts[1]);
                        PlayerStructList.Add(new Player(playerId, hostXP, playXP));
                    }
                }
            }
        }

        internal static string GetAsString()
        {
            return File.ReadAllText(FileName);
        }

        internal static List<Player> GetAll()
        {
            return PlayerStructList.ToList();
        }
    }
}
