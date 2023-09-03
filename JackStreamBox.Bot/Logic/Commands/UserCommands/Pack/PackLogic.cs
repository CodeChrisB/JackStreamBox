using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace JackStreamBox.Bot.Logic.Commands.UserCommands.PackCommand
{
    public class PackLogic
    {
        public static async void DisplayPack(CustomContext context, int pack)
        {
            if (pack > 0 && pack < 10)
            {
                string[] emoji = new string[5] {
                ":one:",
                ":two:",
                ":three:",
                ":four:",
                ":five:"
                };

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"JackboxPack {pack} contains following games");
                JackboxPack packInfo = PackInfo.GetPackInfo(pack);
                for (int i = 0; i < 5; i++)
                {
                    stringBuilder.AppendLine($"{emoji[i]} {packInfo.games[i].Name}");
                    stringBuilder.AppendLine($"*{packInfo.games[i].Description}*\n");

                }

                var message = await context.Channel.SendMessageAsync(stringBuilder.ToString());
                Destroyer.Message(message, DestroyTime.REALLYSLOW);


            }
        }

        internal static async void SendPackScreenshot(CustomContext context)
        {
            await context.Channel.SendMessageAsync("https://media.discordapp.net/attachments/1066085138791932005/1070771921643372564/image.png?width=674&height=902");
        }
    }
}
