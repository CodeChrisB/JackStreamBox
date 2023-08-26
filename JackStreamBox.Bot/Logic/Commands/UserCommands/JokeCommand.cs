using DSharpPlus.CommandsNext.Attributes;
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
        [Description("Get a bad jackbox related pun. (Using ChatGPT)")]
        [Requires(PermissionRole.TRUSTED)]
        public async Task MakeRizz(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED))
            {
                await context.Channel.SendMessageAsync("Wanna hear a joke? You can't use this command.");
            }
            var message = await context.Channel.SendMessageAsync(Joke.GetJoke());

            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            Destroyer.Message(message, DestroyTime.REALLYSLOW);
            
        }
    }
}
