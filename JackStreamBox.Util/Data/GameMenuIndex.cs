using System;
using System.Collections.Generic;

namespace JackStreamBox.Util.Data
{
    internal class GameMenuIndex
    {
        private static Dictionary<string,int> Index = new Dictionary<string,int>() {
            //Pack 1
            {"Ydkj2015" ,0},
            {"Fibbagexl" ,1},
            {"Lieswatter" ,2},
            {"Wordspud" ,3},
            {"Drawful" ,4},
            //Pack 2
            {"Fibbage2" ,0},
            {"Earwax" ,1},
            {"Bidiots" ,2},
            {"Quipplashxl" ,3},
            {"Bombcorp" ,4},
            //Pack 3
            {"Quipplash2" ,0},
            {"Triviamurderparty" ,1},
            {"Guesspionage" ,2},
            {"Fakeinit" ,3},
            {"Teeko" ,4},
            //Pack 4
            {"Fibbage3" ,0},
            {"Surivetheinternet" ,1},
            {"Monstermingle" ,2},
            {"Bracketeering" ,3},
            {"Civic" ,4},
            //Pack 5
            {"Ydkj2018" ,0},
            {"Splittheroom" ,1},
            {"Madversecity" ,2},
            {"Zeepledoome" ,3},
            {"Patentlystupid", 4},
            //Pack 6
            {"Triviamurderparty2" ,0},
            {"Rolemodels" ,1},
            {"Jokeboat" ,2},
            {"Dictionarium" ,3  },
            {"Pushthebutton" ,4},
            //Pack 7
            {"Quipplash3" ,0},
            {"Devilsandthedetails" ,1},
            {"Champedup" ,2},
            {"Talkingpoints" ,3},
            {"Blatherround" ,4},
            //Pack 8
            {"DrawfulAnimate" ,0},
            {"WheelOfEnormousProportions" ,1},
            {"Jobjob" ,2},
            {"Pollmine" ,3},
            {"WeaponsDrawn" ,4},
            //Pack 9
            {"Fibbage4" ,0},
            {"Roomerang" ,1},
            {"Junktopia" ,2},
            {"Nonesensory" ,3},
            {"Quixort" ,4}
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
