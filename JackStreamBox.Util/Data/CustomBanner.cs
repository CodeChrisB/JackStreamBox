using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Util.Data
{
    public static class CustomBanner
    {
        private const string CBFILENAME = "CustomBanner";

        public static void AddBanner(string newBanner)
        {
            List<string> banners = GetBanners().ToList();
            banners.Add(newBanner);
            WriteBanners(banners.ToArray());
            
        }

        public static bool RemoveBanner(int index)
        {
            List<string> banners = GetBanners().ToList();
            if (index >= banners.Count) return false;
            banners.RemoveAt(index);
            WriteBanners(banners.ToArray());
            return true;
        }

        public static string[] GetAllBanner()
        {
            return GetBanners();
        }

        public static string GetRandomBanner()
        {
            string[] banners = GetBanners();
            return banners[new Random().Next(banners.Length)];
        }

        private static string[] GetBanners()
        {
            string[] banners = BotData.ReadCustomData<string[]>(CBFILENAME);
            if(banners == null || banners.Length == 0) return new string[] { "https://media.discordapp.net/attachments/1066085138791932005/1135296119610552350/7u88ip.png" };
             
            return banners;
        }

        private static void WriteBanners(string[] banners)
        {
            BotData.WriteCustomData<string[]>(CBFILENAME, banners);
        }

    }
}
