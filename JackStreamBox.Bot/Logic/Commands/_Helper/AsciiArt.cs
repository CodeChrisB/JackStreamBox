using DSharpPlus;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper
{
    public class AsciiArt
    {

        private static string[] Names = new string[] { "M. Bubble", "Schmitty", "Cookie Materson", "Gene", "CCB", "some Staff Member" };
        public static async void WelcomeMessage(DiscordClient Client)
        {
            var channel = await Client.GetChannelAsync(CommandLevel.IsDevBot ? ChannelId.DevChannel : ChannelId.JackBotVC);
            string name = BotData.ReadData(BotVals.BOT_NAME, "TB1");

            DiscordEmbedBuilder builder = PlainEmbed
                .CreateEmbed()
                .Title($"**{name} is online!**")
                .Color(DiscordColor.Green)
                .DescriptionAddLine("░▀▄░░▄▀░")
                .DescriptionAddLine("▄▄▄██▄▄▄▄▄   ▀█▀▐░▌")
                .DescriptionAddLine("█░░░░░░█▀█   ░█░▐░▌")
                .DescriptionAddLine("█░░░░░░█▀█   ░█░░█")
                .DescriptionAddLine("█▄▄▄▄▄▄███ JackBotTv :red_circle:")
                .GetBuilder();

            await channel.SendMessageAsync(builder.Build());



        }

        private static string StringPadding(string input,int length)
        {
            return input.PadRight(length);
        }

        static string GetName()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, Names.Length);
            return Names[randomIndex];
        }
    }
}
