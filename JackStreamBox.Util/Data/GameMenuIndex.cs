using System;
using System.Collections.Generic;

namespace JackStreamBox.Util.Data
{
    internal class GameMenuIndex
    {
        private static Dictionary<string,int> Index = new Dictionary<string,int>() {
            {"Ydkj2015" ,0},
            {"Fibbagexl" ,1},
            {"Lieswatter" ,2},
            {"Wordspud" ,3},
            {"Drawful" ,4},
            {"Fibbage2" ,0},
            {"Earwax" ,1},
            {"Bidiots" ,2},
            {"Quipplashxl" ,3},
            {"Bombcorp" ,4},
            {"Quipplash2" ,0},
            {"Triviamurderparty" ,1},
            {"Guesspionage" ,2},
            {"Teeko" ,3},
            {"Fakeinit" ,4},
            {"Fibbage3" ,0},
            {"Surivetheinternet" ,1},
            {"Monstermingle" ,2},
            {"Bracketeering" ,3},
            {"Civic" ,4},
            {"Ydkj2018" ,0},
            {"Splittheroom" ,1},
            {"Madversecity" ,2},
            {"Zeepledoome" ,3},
            {"Patentlystupid", 4},
            {"Triviamurderparty2" ,0},
            {"Dictionarium" ,1},
            {"Pushthebutton" ,2},
            {"Jokeboat" ,3},
            {"Rolemodels" ,4},
            {"Quipplash3" ,0},
            {"Devilsandthedetails" ,1},
            {"Champedup" ,2},
            {"Talkingpoints" ,3},
            {"Blatherround" ,4},
            {"DrawfulAnimate" ,0},
            {"WheelOfEnormousProportions" ,1},
            {"Jobjob" ,2},
            {"Pollmine" ,3},
            {"WeaponsDrawn" ,4},
            {"Fibbage4" ,0},
            {"Quixort" ,1},
            {"Junktopia" ,2},
            {"Nonesensory" ,3},
            {"Roomerang" ,4}
        };

        public static int GetIndex(Game gameName)
        {
            string enumName = Enum.GetName(typeof(Game), gameName);

            int val = -1;
            Index.TryGetValue(enumName, out val);

            if (val == -1) throw new NotSupportedException();

            return val;
        } 
    }
}
