using JackBoxStream.Util.data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JackBoxStream.Util.logic
{

    internal class GameOpener
    {

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);


        //Todo do we need any info to open
        public GameOpener() { }

        public async Task<bool> Open(Game game)
        {
            var task = OpenPack(game);

            await task;
            return task.Result;
        }


        static async Task<bool> OpenPack(Game game)
        {
            string path = PackPath(getPackByEnum(game));
            
            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;

            try
            {
                // Start the process
                Process process = Process.Start(startInfo);

                // Wait until the program is fully launched
                if(process == null) return false;
                await Task.Delay(Time.SECOND * 20);

                // Return true if the process was started successfully
                return await NavigateToGame(game);
            }
            catch (Exception ex)
            {
                // An exception occurred, return false
                Console.WriteLine("Failed to start process: " + ex.Message);
                return false;
            }
        }

        private static string PackPath(int pack)
        {

            string path = "The Jackbox Party Pack";
            path += pack > 1 ? " " + pack : "";
            string packName = path;

            path += "\\" + packName + ".exe";
            path = GetSteamPath() + path;
            return path;
        }
        private static int getPackByEnum(Game game)
        {
            switch (game)
            {
                case Game.Ydkj2015:
                case Game.Fibbagexl:
                case Game.Drawful:
                case Game.Wordspud:
                case Game.Lieswatter:
                    return 1;
                case Game.Fibbage2:
                case Game.Earwax:
                case Game.Bidiots:
                case Game.Quipplashxl:
                case Game.Bombcorp:
                    return 2;
                case Game.Quipplash2:
                case Game.Triviamurderparty:
                case Game.Guesspionage:
                case Game.Teeko:
                case Game.Fakeinit:
                    return 3;
                case Game.Fibbage3:
                case Game.Surivetheinternet:
                case Game.Monstermingle:
                case Game.Bracketeering:
                case Game.Civic:
                    return 4;
                case Game.Ydkj2018:
                case Game.Splittheroom:
                case Game.Madversecity:
                case Game.Patentlystupid:
                case Game.Zeepledoome:
                    return 5;
                case Game.Triviamurderparty2:
                case Game.Dictionarium:
                case Game.Pushthebutton:
                case Game.Jokeboat:
                case Game.Rolemodels:
                    return 6;
                case Game.Quipplash3:
                case Game.Devilsandthedetails:
                case Game.Champedup:
                case Game.Talkingpoints:
                case Game.Blatherround:
                    return 7;
                case Game.DrawfulAnimate:
                case Game.WheelOfEnormousProportions:
                case Game.Jobjob:
                case Game.Pollmine:
                case Game.WeaponsDrawn:
                    return 8;
                case Game.Fibbage4:
                case Game.Quixort:
                case Game.Junktopia:
                case Game.Nonesensory:
                case Game.Roomerang:
                    return 9;
            }
            throw new KeyNotFoundException();
        }

        private static string GetSteamPath()
        {
            return "D:\\SteamLibrary\\steamapps\\common\\";
        }

        static async Task<bool> NavigateToGame(Game game)
        {
            string windowName = "The Jackbox Party Pack";
            windowName += getPackByEnum(game) > 1 ? " " + getPackByEnum(game) : "";
            WindowNavigator navigator = new WindowNavigator(windowName);

            string[] inputs = InputGenerator.Generate(game);
            foreach (string input in inputs)
            {
;
                Console.WriteLine($"Performed ${input};Now waiting");

                navigator.SendInput(input);
                await Task.Delay(Time.SECOND * 8);
            }
            return true;
        }
    }
}
