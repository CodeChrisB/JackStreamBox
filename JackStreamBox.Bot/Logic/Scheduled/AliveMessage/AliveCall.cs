using DSharpPlus.Entities;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Scheduled.Overwatch;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Data;

namespace JackStreamBox.Bot.Logic.Scheduled.AliveMessage
{
	internal class AliveCall
	{

		public static DiscordMessage message { get; set; }
		public static async Task CheckIfNeeded()
		{
			//if vc bigger 1 return
			try {
				if (OverwatchVC.JackBotCount > 2) return;

				if (message != null)
				{
					Destroyer.Message(message, DestroyTime.INSTANT);
				}

				var channel = await Bot.Client.GetChannelAsync(ChannelId.JackBotVC);

				var builder = PlainEmbed.CreateEmbed()
					.Author("About the host", "https://cdn.discordapp.com/avatars/1102691400648237076/8fac5d70bcd5a2daebd4540202417dbf.webp?size=80")
					.DescriptionAddLine("KirbyTV is a bot that hosts Jackbox for you")
					.DescriptionAddLine("**/menu** Opens a menu so that u can start a game")
					.DescriptionAddLine("**/commands** Shows all commands you can use")
					.DescriptionAddLine("**/report [Message]** Found a bug? Report it")
					.DescriptionAddLine("\nWe are happy that you are concerned about the bot's health")
					.DescriptionAddLine("KirbyTV get's alot of treats every day !")
					.Color(DiscordColor.HotPink)
					.GetBuilder();


				message = await channel.SendMessageAsync(builder.Build());

				//send message
			}catch (Exception ex)
			{

			}

		}

	}
}
