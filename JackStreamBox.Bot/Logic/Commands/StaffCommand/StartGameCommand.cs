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
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    public class StartGameCommand : BaseCommandModule
    {
        [Command("start")]
        [Description("Starts any game you want to. Be aware this instantly closes any current game!")]
        //Dont docuemnt this command
        [ModCommand(PermissionRole.STAFF)]
        public async Task OpenGame(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;


            var row1 = genRow(new int[] { 1, 2, 3, 4, 5 }, "Choose a pack");
            var message1 = await context.Channel.SendMessageAsync(row1).ConfigureAwait(false);
            var row2 = genRow(new int[] { 6, 7, 8, 9, 10 }, "------------");
            var message2 = await context.Channel.SendMessageAsync(row2).ConfigureAwait(false);
            DiscordMessage gamePickerMessage = null;

            async Task Logger(VoteStatus status)
            {
                //await context.Channel.SendMessageAsync(message).ConfigureAwait(false);
            }


            Bot.Client.ComponentInteractionCreated += async (s, e) =>
            {
                if (e.User.Id != context.User.Id) return;

                string[] command = e.Id.ToString().Split("-");

                switch (command[0])
                {
                    case "pack":
                        await message1.DeleteAsync().ConfigureAwait(false);
                        await message2.DeleteAsync().ConfigureAwait(false);
                        var gamePicker = OpenPack(command[1]);
                        gamePickerMessage = await context.Channel.SendMessageAsync(gamePicker).ConfigureAwait(false);
                        break;
                    case "game":
                        Game gameId = (Game)Enum.Parse(typeof(Game), command[1]);
                        if (gamePickerMessage != null) await gamePickerMessage.DeleteAsync();
                        await JackStreamBoxUtility.OpenGame(gameId, Logger);
                        break;
                }
            };
        }

        [Command("join")]
        [Description("Let the streamer join the VC.")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task Join(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            JackStreamBoxUtility.JoinLobby();
            var message = await context.Channel.SendMessageAsync("Done.");
            Destroyer.Message(message, DestroyTime.REALLYSLOW);
        }

        [Command("closegame")]
        [Description("Closes the current game.")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task CloseGame(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            JackStreamBoxUtility.CloseGame();
            var message = await context.Channel.SendMessageAsync("Closed the game !");
            Destroyer.Message(message, DestroyTime.FAST);
        }

        [Command("close")]
        [Description("Will close the bot. Use in emergency such as the bot streaming a wrong window.")]
        //Dont docuemnt this command
        [ModCommand(PermissionRole.DEVELOPER)]
        public async Task Close(CommandContext context)
        {
            //todo
            Environment.Exit(0);
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

        private DiscordMessageBuilder genRow(int[] pack, string text)
        {

            var content = new DiscordComponent[pack.Length];

            for (int i = 0; i < content.Length; i++)
            {
                content[i] = new DiscordButtonComponent(ButtonStyle.Primary, $"pack-{pack[i].ToString()}", $"Pack {pack[i].ToString()}");
            }

            var builder = new DiscordMessageBuilder()
                .WithContent(text)
                .AddComponents(content);


            return builder;
        }

    }
}
