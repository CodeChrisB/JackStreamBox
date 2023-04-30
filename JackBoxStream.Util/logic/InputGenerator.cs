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

            
            List<string> inputs = new List<string>{Input.ENTER};
            //0 nothing
            //1 down
            if(menuIndex == 1) inputs.Add(Input.ARROW_DOWN);
            //2 down down
            if (menuIndex == 2)
            {
                inputs.Add(Input.ARROW_DOWN);
                inputs.Add(Input.ARROW_DOWN);
            }

            //3 up
            if (menuIndex == 3)
            {
                inputs.Add(Input.ARROW_UP);
                inputs.Add(Input.ARROW_UP);
            }

            //4 up up
            if (menuIndex == 4) inputs.Add(Input.ARROW_UP);

            //2x enter press => open game , start game
            inputs.Add(Input.ENTER);
            inputs.Add(Input.ENTER);
            return inputs.ToArray();
        }
    }
}
