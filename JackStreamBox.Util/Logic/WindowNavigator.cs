using JackStreamBox.Util.Logic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JackStreamBox.Util.logic
{
    internal class WindowNavigator
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

        public static bool SendeGameInput(String input)
        {
            
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

            //We send the input to every instance of discord hoping we hit the correct one, but we dont know...
            SetForegroundWindow(DiscordProcess.MainWindowHandle);
            SendKeys.SendWait(input);

            return true;
        }

        public static bool Close()
        {
            if (GameProcess == null) return false;
            try { 
                GameProcess.Kill();
                return true;
            } 
            catch { 
                return false;
            }
        }

        public static string getMemory()
        {
            Sniffer.SetUp();
            return "";
        }


    }
}
