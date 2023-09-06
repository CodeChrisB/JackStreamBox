using DSharpPlus;
using DSharpPlus.EventArgs;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Voting;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands
{
    internal class ButtonHandler
    {
        internal static async Task OnInteraction(DiscordClient s, ComponentInteractionCreateEventArgs e)
        {
            string id = e.Id;

            DateTime dt = e.Interaction.CreationTimestamp.DateTime;

            switch(id)
            {
                case ButtonId.PACK1:
                case ButtonId.PACK2:
                case ButtonId.PACK3:
                case ButtonId.PACK4:
                case ButtonId.PACK5:
                case ButtonId.PACK6:
                case ButtonId.PACK7:
                case ButtonId.PACK8:
                case ButtonId.PACK9:
                case ButtonId.PACK10:
                    VoteLogic.VoteViaMenu(e.ToCustomContext(),id,dt);
                    break;


            }
        }
    }
}
