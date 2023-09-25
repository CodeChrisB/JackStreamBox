using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.DevCommands
{
	internal class MakeGamepad : BaseCommandModule
	{
		[Command("gamepad")]
		[CoammandDescription("Creates the buttons", ":rocket:")]
		[ModCommand(PermissionRole.DEVELOPER)]
		public async Task Update(CommandContext context)
		{
		}
	}
}
