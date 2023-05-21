using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Util.Data;
namespace JackStreamBox.Bot.Logic.Commands
{
    public class InputCommand : BaseCommandModule
    {

        [Command("break")]
        [Description("Pauses the game for one minute")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task PeeBreak(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            WindowNavigator.SendGameInput(Input.ESCAPE);
            int time = 60;
            var message = await context.Channel.SendMessageAsync($"Pausing the game for {time} seconds.");
            while(time > 0)
            {
                Task.Delay(TimeSpan.FromSeconds(10)).Wait();
                time -= 10;
                await message.ModifyAsync(MessageGenerator(time));
            }

            await message.ModifyAsync("Resuming game");
            WindowNavigator.SendGameInput(Input.ESCAPE);
        }

        [Command("input")]
        [Description("Let's you navigate the game yourself. Used to change settings.")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task RealInput(CommandContext context,string input)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
            DoInput(input, 1);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
        }

        [Command("input")]
        public async Task RealInput(CommandContext context, string input,int times)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
            DoInput(input, times);
            Destroyer.Message(context.Message,DestroyTime.INSTANT);
        }

        private void DoInput(string input, int times )
        {
            string? internalInput = null;
            switch (input.ToLower())
            {
                case "esc":
                case "escape":
                    internalInput = Input.ESCAPE; break;
                case "up":
                case "u":
                    internalInput = Input.ARROW_UP; break;
                case "down":
                case "d":
                    internalInput = Input.ARROW_DOWN; break;
                case "left":
                case "l":
                    internalInput = Input.ARROW_LEFT; break;
                case "right":
                case "r":
                    internalInput = Input.ARROW_RIGHT; break;
                case "enter":
                    internalInput = Input.ENTER; break;
            }
            //No setting requires more than 20 presses (I dont think more than 10 are needed)
            if (times > 20) times = 20;
            for(int i = 0; i < times && internalInput != null; i++)
            {
                WindowNavigator.SendGameInput(internalInput);
            }

           

        }

        private string MessageGenerator(int time) {
            return $"Game will resume in {time} seconds.";
        }
    }
}
