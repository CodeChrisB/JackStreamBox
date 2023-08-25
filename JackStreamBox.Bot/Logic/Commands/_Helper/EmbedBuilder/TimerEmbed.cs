using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    internal class TimerEmbed
    {

        private static Dictionary<string,TimerEmbedData> EmbedData = new ();

        public static async Task<DiscordMessage> Show(CommandContext context, TimerEmbedData timerData)
        {
            EmbedData.Add(timerData.Key, timerData);
            EmbedData[timerData.Key].Builder.Title = timerData.Title;
           

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


            if (EmbedData[key].Embed == null)
            {
                EmbedData[key].Builder.Description = content.ToString();
                EmbedData[key].Builder.Title = EmbedData[key].Title;
                EmbedData[key].Embed = await EmbedData[key].Context.Channel.SendMessageAsync(embed: EmbedData[key].Builder).ConfigureAwait(false);
            }
            else
            {
                await EmbedData[key].Embed.ModifyAsync(EmbedData[key].Builder.Build());
            }
        }

        private static DiscordEmbedBuilder BuilderHelper()
        {
            return new DiscordEmbedBuilder
            {
                Title = "",
                Description = "",
                Color = DiscordColor.Green,

            };
        }
        public static void UpdateCustom(string key, string custom) => EmbedData. = custom;
    }
}
