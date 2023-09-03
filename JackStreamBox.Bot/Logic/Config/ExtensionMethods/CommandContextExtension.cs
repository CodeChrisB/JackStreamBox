using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Commands._Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Config.ExtensionMethods
{
    public static class CommandContextExtension
    {
        public static CustomContext ToCustomContext(this CommandContext context)
        {
            return new CustomContext(context);
        }


        public static CustomContext ToCustomContext(this InteractionContext context)
        {
            return new CustomContext(context);
        }
    }
}
