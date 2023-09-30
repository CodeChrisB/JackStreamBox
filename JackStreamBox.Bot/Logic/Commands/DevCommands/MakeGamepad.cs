using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JackStreamBox.Bot.Logic.Commands.DevCommands
{
	internal class MakeGamepad : BaseCommandModule
	{
		[Command("gamepad")]
		[CoammandDescription("Creates the buttons", ":rocket:")]
		[ModCommand(PermissionRole.DEVELOPER)]
		public async Task Update(CommandContext context,string windowname, [RemainingText] string message)
		{
			if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
			// Name,Key Name,Key Name,Key

			WindowNavigator.SetCustomGame(windowname);

			List<DiscordButtonComponent> buttons = GetButtons(message);

			var chunks = buttons.Select((value, index) => new { value, index })
					  .GroupBy(pair => pair.index / 5, pair => pair.value);



			var builder = new DiscordMessageBuilder();
			builder.WithContent("Gamepad");






			foreach (var chunk in chunks)
			{
				Console.WriteLine("Processing chunk:");
				builder.AddComponents(chunk);
			}


			await context.Channel.SendMessageAsync(builder);




		}

		private List<DiscordButtonComponent> GetButtons(string message)
		{
			string[] buttonsDefinition = message.Split(' ');

			List<DiscordButtonComponent> discordButtons = new List<DiscordButtonComponent>();
			foreach (string button in buttonsDefinition)
			{
				string[] btnDefinition = button.Split(":");

				if (btnDefinition.Length <2) continue;

				int time = btnDefinition.Length == 3 ? int.Parse(btnDefinition[2]) : 5;

				discordButtons.Add(new DiscordButtonComponent(ButtonStyle.Secondary, $"gamepad-{btnDefinition[1]}-{time}", btnDefinition[0], false));
			}

			return discordButtons;
		}
	}
}
