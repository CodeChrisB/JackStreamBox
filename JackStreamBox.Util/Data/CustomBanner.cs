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
            List<string> banners = GetBanners();
            banners.Add(newBanner);
            WriteBanners(banners);
            
        }

        public static void RemoveBanner(int index)
        {
            List<string> banners = GetBanners();
            banners.RemoveAt(index);
            WriteBanners(banners);
        }

        public static string[] GetAllBanner()
        {
            return GetBanners().ToArray();
        }

        public static string GetRandomBanner()
        {
            List<string> banners = GetBanners();
            return banners[new Random().Next(banners.Count)];
        }

        private static List<string> GetBanners()
        {
            List<string> banners = BotData.ReadCustomData<List<string>>(CBFILENAME);
            if(banners == null || banners.Count == 0) return new List<string> { "https://media.discordapp.net/attachments/1066085138791932005/1135296119610552350/7u88ip.png" };
             
            return banners;
        }

        private static void WriteBanners(List<string> banners)
        {
            BotData.WriteCustomData<List<string>>(CBFILENAME, banners);
        }

    }
}
