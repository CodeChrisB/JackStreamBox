using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Gamepad;
using JackStreamBox.Bot.Logic.Commands.UserCommands.Voting;
using JackStreamBox.Bot.Logic.Config;
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
			if (id.StartsWith("gamepad")) GamepadLogic.OnGamePadClick(id);

            CustomContext context = e.ToCustomContext();
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

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
                    VoteLogic.VoteViaMenu(context, id,dt);
                    break;
                case ButtonId.VOTE1:
                case ButtonId.VOTE2:
                case ButtonId.VOTE3:
                case ButtonId.VOTE4:
                case ButtonId.VOTE5:
                    VoteLogic.OnGameVote(context, id);
                    break;
            }

           
        }
    }
}
