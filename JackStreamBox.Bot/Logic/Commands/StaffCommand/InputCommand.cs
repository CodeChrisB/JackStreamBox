﻿using DSharpPlus.CommandsNext;
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

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    public class InputCommand : BaseCommandModule
    {

        [Command("break")]
        [CoammandDescription("Pauses the game for 1 minute, will resume afterwards.",":play_pause:")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task PeeBreak(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            WindowNavigator.SendGameInput(Input.ESCAPE);
            int time = 60;
            var message = await context.Channel.SendMessageAsync($"Pausing the game for {time} seconds.");
            
            while (time > 0)
            {
                Task.Delay(1000).Wait();
                time -= 1;
                await message.ModifyAsync(MessageGenerator(time));
            }

            await message.ModifyAsync("Resuming game");
            WindowNavigator.SendGameInput(Input.ESCAPE);
        }

        [Command("input")]
        [CoammandDescription("Let's you navigate the game yourself. Used to change settings.",":clipboard:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task RealInput(CommandContext context, string input)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            DoInput(input, 1);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
        }

        [Command("input")]
        public async Task RealInput(CommandContext context, string input, int times)
        {

            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            DoInput(input, times);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
        }

        [Command("w")] public async Task winput(CommandContext context) => await RealInput(context, "u");
        [Command("a")] public async Task ainput(CommandContext context) => await RealInput(context, "l");
        [Command("s")] public async Task sinput(CommandContext context) => await RealInput(context, "d");
        [Command("d")] public async Task dinput(CommandContext context) => await RealInput(context, "r");
        [Command("esc")] public async Task escinput(CommandContext context) => await RealInput(context, "esc");
        [Command("enter")] public async Task enterinput(CommandContext context) => await RealInput(context, "enter");

        [Command("w")] public async Task winput(CommandContext context, int times) => await RealInput(context, "u",times);
        [Command("a")] public async Task ainput(CommandContext context, int times) => await RealInput(context, "l",times);
        [Command("s")] public async Task sinput(CommandContext context, int times) => await RealInput(context, "d",times);
        [Command("d")] public async Task dinput(CommandContext context, int times) => await RealInput(context, "r",times);
        [Command("esc")] public async Task escinput(CommandContext context, int times) => await RealInput(context, "esc", times);
        [Command("enter")] public async Task enterinput(CommandContext context, int times) => await RealInput(context, "enter", times);

        private void DoInput(string input, int times)
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
            for (int i = 0; i < times && internalInput != null; i++)
            {
                WindowNavigator.SendGameInput(internalInput);
            }



        }

        private string MessageGenerator(int time)
        {
            return $"Game will resume in {time} seconds.";
        }
    }
}
