using JackBoxStream.Util.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackBoxStream.Util.logic
{
    internal class InputGenerator
    {

        //enter -> main menu
        //down til game
        //enter -> activate game
        //enter ->start game

        public static string[] Generate(Game game)
        {
            int menuIndex = GameMenuIndex.GetIndex(game);
            string[] inputs = new string[3+menuIndex];

            Array.Fill(inputs, Input.ARROW_DOWN);
            inputs[0] = Input.ENTER;
            inputs[inputs.Length-2] = Input.ENTER;
            inputs[inputs.Length-1] = Input.ENTER;
            return inputs;
        }
    }
}
