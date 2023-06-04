using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Data
{
    public class PieChart
    {
        public static string GenerateLink(IReadOnlyCollection<Reaction> reactions, PackGame[] games,DiscordEmoji[] emoji)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("https://image-charts.com/chart?cht=p3&chs=300x300");
            string gameVotes = "";
            for (int i=0;i<reactions.Count;i++)
            {
                gameVotes += $"{reactions.ElementAt(i).Total},";
            }
            sb.Append($"&chd=t:{gameVotes}");

            string gameNames = "";

            for (int i = 0; i < reactions.Count; i++)
            {
                int index = -1;
                index = emoji.ToList().IndexOf(reactions.ElementAt(i).Emoji);
                if(index > -1)
                {
                    gameNames += $"{games[index].Name.Replace(" ","")}|";
                }
            }
            sb.Append($"&chl={gameNames}");
            sb.Append("&chan");
            sb.Append("&chf=ps0-0,lg,45,ffeb3b,0.2,f44336,1|ps0-1,lg,45,8bc34a,0.2,009688,1");
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
    }
}
