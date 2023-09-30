using JackStreamBox.Util.logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Gamepad
{
	public class GamepadLogic
	{
		public  static void OnGamePadClick(string button)
		{
			string[] idParts = button.Split('-');
			if (idParts.Length < 2) return;
			int time = idParts.Length == 3 ? int.Parse(idParts[2]) : 5;
			WindowNavigator.SendCustomGameInput(idParts[1], time);
		}
	}
}
