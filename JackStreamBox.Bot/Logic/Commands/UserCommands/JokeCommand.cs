﻿using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config;

namespace JackStreamBox.Bot.Logic.Commands
{
    public class JokeCommand : BaseCommandModule 
    {
        [Command("joke")]
        [CoammandDescription("Get a bad jackbox related pun. (Using ChatGPT)",":tada:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task MakeRizz(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE))
            {
                await context.Channel.SendMessageAsync("Wanna hear a joke? You can't use this command.");
                return;
            }
            var message = await context.Channel.SendMessageAsync(Joke.GetJoke());

            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            Destroyer.Message(message, DestroyTime.REALLYSLOW);
            
        }
    }
}
