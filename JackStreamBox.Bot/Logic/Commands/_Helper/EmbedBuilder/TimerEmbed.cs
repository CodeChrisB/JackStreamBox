using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder
{

    public class TimerEmbedData
    {
        public CommandContext Context;
        public string Title = "";
        public string Before = "";
        public string Custom = "";
        public string After = "";
        public string Key = "";
        public int Time;
        public DiscordMessage Embed;
        public DiscordEmbedBuilder Builder;

        public void DecrementTimer()
        {
            Time--;
        }
    }
    public static class TimerEmbed
    {

        private static Dictionary<string,TimerEmbedData> EmbedData = new ();

        public static async Task<DiscordMessage> Show(CommandContext context, TimerEmbedData timerData)
        {
            EmbedData.Add(timerData.Key, timerData);
            await BuilderHelper(timerData.Key);

            while(EmbedData[timerData.Key].Time > 0)
            {

                ModifyEmbed(timerData.Key);
                EmbedData[timerData.Key].DecrementTimer();
                //a cycle takes longer than a second due to waiting for response time.. just sayin..
                await Task.Delay(1000);
            }



            await EmbedData[timerData.Key].Embed.DeleteAsync();

            DiscordMessage returnVal = EmbedData[timerData.Key].Embed;
            EmbedData.Remove(timerData.Key);
            return returnVal;
        }

        private async static void ModifyEmbed(string key)
        {
           StringBuilder content = new StringBuilder();

            content.AppendLine($"Time Left: { EmbedData[key].Time}s");

            content.AppendLine(EmbedData[key].Before);
            content.AppendLine(EmbedData[key].Custom);
            content.AppendLine(EmbedData[key].After);

            EmbedData[key].Builder.Title = EmbedData[key].Title;
            EmbedData[key].Builder.Description = content.ToString();

            await EmbedData[key].Embed.ModifyAsync(EmbedData[key].Builder.Build());
        }

        private async static Task BuilderHelper(string key)
        {
            EmbedData[key].Builder = new DiscordEmbedBuilder
            {
                Title = "",
                Description = "",
                Color = DiscordColor.Green,

            };

            EmbedData[key].Embed = await EmbedData[key].Context.Channel.SendMessageAsync(embed: EmbedData[key].Builder).ConfigureAwait(false);
        }
        public static void UpdateCustom(string key, string custom)
        {
            if(EmbedData.ContainsKey(key))
            {
                EmbedData[key].Custom = custom;
            }
        }
    }
}
