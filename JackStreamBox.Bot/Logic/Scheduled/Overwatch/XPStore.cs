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
        public static ulong AddXp(ulong id)
        {
            LoadDataFromFile();
            ulong xpToAdd = 10;
            if(XPStoreData.ContainsKey(id))
            {
                XPStoreData[id] += xpToAdd;
            }
            else
            {
                XPStoreData.Add(id, xpToAdd);
            }
            SaveDataToFile();

            return xpToAdd;
        }

        public static ulong GetById(ulong id)
        {
            LoadDataFromFile();
            return XPStoreData[id];
        }

        public static void GetTop(int n)
        {
            //gets top n user by xp
        }


        /* End Of Public API */

        const string FileName = "xpSheet";
        private static Dictionary<ulong, ulong> XPStoreData = new Dictionary<ulong, ulong>();
        private static void SaveDataToFile()
        {
            using (StreamWriter writer = new StreamWriter(FileName))
            {
                foreach (var data in XPStoreData)
                {
                    writer.WriteLine($"{data.Key},{data.Value}");
                }
                writer.Close();
            }
        }

        private static void LoadDataFromFile()
        {
            XPStoreData = new Dictionary<ulong, ulong>();

            if (File.Exists(FileName))
            {
                string[] lines = File.ReadAllLines(FileName);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        var key = ulong.Parse(parts[0]);
                        var value = ulong.Parse(parts[1]);
                        XPStoreData[key] = value;
                    }
                }
            }
        }


    }
}
