using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Attributes;
using DSharpPlus;
using static System.Net.Mime.MediaTypeNames;
using JackStreamBox.Util.Data;

namespace JackStreamBox.Bot.Logic.Commands
{
    public class StartGameCommand : BaseCommandModule
    {
        [Command("start")]
        [Description("Opens any game you want. Uses the game position.\nMad Verse city is the 3rd game in the 5th pack it's position is (5*5+3=28)\n  !closes any game that currently is held.")]
        [Requires(PermissionRole.STAFF)]
        public async Task OpenGame(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;


            var row1 = genRow(new int[] { 1, 2, 3, 4, 5 },"Choose a pack");
            var message1 = await context.Channel.SendMessageAsync(row1).ConfigureAwait(false);
            var row2 = genRow(new int[] { 6, 7, 8, 9, 10 },"------------");
            var message2 = await context.Channel.SendMessageAsync(row2).ConfigureAwait(false);
            DiscordMessage gamePickerMessage = null;

            async Task Logger(string message)
            {
                await context.Channel.SendMessageAsync(message).ConfigureAwait(false);
            }


            Bot.Client.ComponentInteractionCreated += async (s, e) =>
            {
                string[] command = e.Id.ToString().Split("-");

                switch (command[0])
                {
                    case "pack":
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                        await message1.DeleteAsync().ConfigureAwait(false);
                        await message2.DeleteAsync().ConfigureAwait(false);
                        var gamePicker = OpenPack(command[1]);
                        gamePickerMessage = await context.Channel.SendMessageAsync(gamePicker).ConfigureAwait(false);
                        break;
                    case "game":
                        Game gameId = (Game)Enum.Parse(typeof(Game), command[1]);
                        await gamePickerMessage.DeleteAsync();
                        await JackStreamBoxUtility.OpenGame(gameId, Logger);
                        break;
                }
            };






            //var task = JackStreamBoxUtility.OpenGame((Util.Data.Game)game-1,Logger);
            //await task;
        }

        [Command("join")]
        [Description("Let the streamer join the VC.")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task Join(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            JackStreamBoxUtility.JoinLobby();
            await context.Channel.SendMessageAsync("Done.");
        }

        //HELPERS

        private DiscordMessageBuilder OpenPack(string id)
        {
            Pack chosenPack = PackInfo.GetPackInfo(int.Parse(id));

            var content = new DiscordComponent[5];

            for (int i = 0; i < content.Length; i++)
            {
                content[i] = new DiscordButtonComponent(ButtonStyle.Primary, $"game-{chosenPack.games[i].Id}", chosenPack.games[i].Name);
            }

            var builder = new DiscordMessageBuilder()
                .WithContent("Pick a Game")
                .AddComponents(content);
            return builder;
        }

        private DiscordMessageBuilder genRow(int[] pack,string text)
        {

            var content = new DiscordComponent[pack.Length];

            for (int i = 0; i<content.Length; i++)
            {
                content[i] = new DiscordButtonComponent(ButtonStyle.Primary, $"pack-{(pack[i]).ToString()}", $"Pack {(pack[i]).ToString()}");
            }

            var builder = new DiscordMessageBuilder()
                .WithContent(text)
                .AddComponents(content);


            return builder;
        }

    }
}
