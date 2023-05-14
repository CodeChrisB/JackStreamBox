using JackStreamBox.Util.Logic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JackStreamBox.Util.logic
{
    public class WindowNavigator
    {
        [DllImport("User32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr point);

        public static string WindowName {get;private set;}
        public static Process GameProcess { get;private set;}
        public static Process DiscordProcess { get; private set; }

        public static void SetWindow(string windowName)
        {
            WindowName = windowName;
            Process[] ps = Process.GetProcessesByName(windowName);

            GameProcess = ps.FirstOrDefault();
        }

        public static void SetDiscord()
        {
            Process[] ps = Process.GetProcessesByName("Discord");
            DiscordProcess = ps.FirstOrDefault();
        }

        public static bool SendGameInput(String input)
        {
            
            if (GameProcess == null) SetDiscord();
            if (GameProcess == null) return false;

            //bring the window to the foreground
            IntPtr h = GameProcess.MainWindowHandle;
            SetForegroundWindow(h);

            SendKeys.SendWait(input);
            return true;
        }

        public static bool SendDiscordInput(String input)
        {

            if (DiscordProcess == null) return false;

            IntPtr h = DiscordProcess.MainWindowHandle;

            SetForegroundWindow(h);
            SendKeys.SendWait(input);

            return true;
        }

        public static void Close()
        {
            for(int i = 1; i < 11; i++)
            {
                string windowName = "The Jackbox Party Pack";
                if (i > 1) windowName += $" {i}";
                Process game = Process.GetProcessesByName(windowName).FirstOrDefault();
                if(game != null)
                {
                    //Usually just killing the gameProcess is enough but after and crash or something
                    //Multiple jackbox party games could be opend so killing all of them will prevent anything like this.
                    game.Kill();
                }

            }
        }

        public static string getMemory()
        {
            Sniffer.SetUp();
            return "";
        }


    }
}
