using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Util.Data
{
    public static class ListSerializer
    {
        public const string BANNER = "jsbIgnoreCustomBanner";
        public const string RULE = "jsbIgnoreRules";

        public static void AddEntry(string file, string newEntry)
        {
            List<string> banners = GetEntry(file).ToList();
            banners.Add(newEntry);
            WriteEntry(file,banners.ToArray());
            
        }

        public static bool RemoveEntry(string file,int index)
        {
            List<string> banners = GetEntry(file).ToList();
            banners.RemoveAt(index);
            WriteEntry(file, banners.ToArray());
            return true;
        }

        public static string[] GetList(string file)
        {
            return GetEntry(file);
        }

        public static string GetRandomEntry(string file)
        {
            string[] banners = GetEntry(file);
            return banners[new Random().Next(banners.Length)];
        }

        private static string[] GetEntry(string file)
        {
            string[] banners = BotData.ReadCustomData<string[]>(file);
            if(banners == null || banners.Length == 0) return new string[] { "https://media.discordapp.net/attachments/1066085138791932005/1135296119610552350/7u88ip.png" };
             
            return banners;
        }

        private static void WriteEntry(string file ,string[] banners)
        {
            BotData.WriteCustomData<string[]>(file, banners);
        }

    }
}
