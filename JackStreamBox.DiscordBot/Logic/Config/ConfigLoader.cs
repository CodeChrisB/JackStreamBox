using JackStreamBox.Bot.Logic.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JackStreamBox.DiscordBot.Logic.Config
{
    public class ConfigLoader
    {
        public async static Task<BotConfig> GetConfig()
        {
            var task = Load();
            await task;

            return task.Result;
        }

        private async static Task<BotConfig> Load()
        {
            var json = String.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();
            return JsonConvert.DeserializeObject<BotConfig>(json);
        }
    }
}
