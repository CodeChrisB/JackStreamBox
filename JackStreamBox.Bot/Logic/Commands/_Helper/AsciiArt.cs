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
        public static string WelcomeMessage(string botname)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("` _____________________________________________________________________`");
            sb.AppendLine("`|[x] JackBot Console                                         | F]| !\"|`");
            sb.AppendLine("`| ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ |`");
            sb.AppendLine($"`|{StringPadding(" > Hey " +botname+ " is now Online !",69)}|`");
            sb.AppendLine($"`|{StringPadding($" > I was busy talking with {GetName()}",69)}|`");
            sb.AppendLine("`| > Use !help if you want to know what I can do                       |`");
            sb.AppendLine("`| > Use !commands to see all commands                             |`");
            sb.AppendLine("`| ___________________________________________________________________ |`");

            return sb.ToString();
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
