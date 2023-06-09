﻿using JackStreamBox.Util.Data;
using System;

namespace JackStreamBox.Util.logic
{
    internal class InputGenerator
    {


        public static string[] Generate(Game game)
        {
            int menuIndex = GameMenuIndex.GetIndex(game);
            string[] inputs = new string[4 + menuIndex];

            Array.Fill(inputs, Input.ARROW_DOWN);
            inputs[0] = Input.ENTER;
            inputs[inputs.Length - 3] = Input.ENTER;
            inputs[inputs.Length - 2] = Input.ENTER;
            inputs[inputs.Length - 1] = Input.ENTER;
            //Make game fullscreen

            return inputs;
        }
    }
}
