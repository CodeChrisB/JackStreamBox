using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace JackStreamBox.Util.logic
{

    internal class GameOpener
    {

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);


        public static async Task<bool> Open(Game game, Func<VoteStatus, Task> Logger)
        {
            var task = OpenPack(game,Logger);

            await task;
            return task.Result;
        }

        static async Task<bool> OpenPack(Game game, Func<VoteStatus, Task> Logger)
        {
            string path = PackPath(getPackByEnum(game));
            
            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;
            int time = 25;
            try
            {
                // Start the process
                Process process = Process.Start(startInfo);

                // Wait until the program is fully launched
                if(process == null) return false;
                await Logger(VoteStatus.OnStartingGamePack);
                await Task.Delay(Time.OpenSteamGame);

                // Return true if the process was started successfully
                return await NavigateToGame(game, Logger);
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

        //Todo save & get SteamPath from SettingsFile
        private static string GetSteamPath()
        {
            return "C:\\Program Files (x86)\\Steam\\steamapps\\common\\";
        }

        private static int width;
        private static int height;

        public static void SetWindowPos()
        {
            CalculateDimensions();
            WindowNavigator.MoveGameWindow(0, 0, width, height, true);
        }

        public static void CalculateDimensions()
        {
            int step = BotData.ReadData("screen", 100);

            if (step > 250) step = 250;
            if (step < 50) step = 50;

            double scaleFactor = step / 100.0;

            int originalWidth = 640;
            int originalHeight = 350;

            width = (int)(originalWidth * scaleFactor);   
            height = (int)(originalHeight * scaleFactor); 
        }


        static async Task<bool> NavigateToGame(Game game,Func<VoteStatus, Task> Logger)
        {
            string windowName = "The Jackbox Party Pack";
            windowName += getPackByEnum(game) > 1 ? " " + getPackByEnum(game) : "";
            
            WindowNavigator.SetWindow(windowName);
            await Task.Delay(150);

            SetWindowPos();

            await Logger(VoteStatus.OnOpendGamePack);
            await Logger(VoteStatus.OnStartingGame);


            string[] inputs = InputGenerator.Generate(game);
            for(int i=0;i<inputs.Length;i++)
            {
                WindowNavigator.SendGameInput(inputs[i]);

                int time = Time.NavigateToGame;
                if (i == 0)
                {
                    //menu open
                    time = Time.OpenGamePicker;
                }else if(i>= inputs.Length - 4)
                {
                    time = Time.StartGame;
                }
                await Task.Delay(time);
                Console.WriteLine($"Performed ${inputs[i]};Now waiting {time}ms");
            }

            await Logger(VoteStatus.OnGameOpend);
            await Logger(VoteStatus.OnStartingStream);

            //Start Stream
            WindowNavigator.SetDiscord();
            WindowNavigator.SendDiscordInput(Input.DiscordKey);


            await Logger(VoteStatus.OnAllFinished);

            return true;
        }
    }
}
