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
        public static int AddXp(ulong id)
        {
            
            Random random = new Random(DateTime.Now.Millisecond); 
            int xpToAdd = BotData.ReadData(BotVals.XP_AMOUNT,10);
            xpToAdd += random.Next(0, BotData.ReadData(BotVals.XP_RANDOM, 0));
            if(XPStoreData.ContainsKey(id.ToString()))
            {
                XPStoreData[id.ToString()] += xpToAdd;
            }
            else
            {
                XPStoreData.Add(id.ToString(), xpToAdd);
            }
            SaveDataToFile();

            return xpToAdd;
        }

        public static int GetById(ulong id)
        {
            return XPStoreData[id.ToString()];
        }
        public static int GetPosById(ulong id)
        {
            return XPStoreData.ToList().OrderBy(user => user.Value).ToList().FindIndex(pair => pair.Key == id.ToString());
        }

        public static IEnumerable<KeyValuePair<string, int>> GetTop(int n)
        {
            //gets top n user by xp

            return XPStoreData.ToList().OrderByDescending(kv => kv.Value).Take(n);

        }

        internal static void DELETEALLXP()
        {
            XPStoreData = new Dictionary<string, int>();
            SaveDataToFile(); // :(
        }

        /* End Of Public API */

        const string FileName = "xpSheet";
        private static Dictionary<string, int> XPStoreData = new Dictionary<string, int>();
        private static void SaveDataToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    foreach (var data in XPStoreData)
                    {
                        writer.WriteLine($"{data.Key},{data.Value}");
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
            XPStoreData = new Dictionary<string, int>();

            if (File.Exists(FileName))
            {
                string[] lines = File.ReadAllLines(FileName);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        var key = parts[0];
                        var value = int.Parse(parts[1]);
                        XPStoreData[key] = value;
                    }
                }
            }
        }

        internal static string GetAsString()
        {
            return File.ReadAllText(FileName);
        }

        internal static Dictionary<string, int> GetAll()
        {
            return XPStoreData;
        }
    }
}
