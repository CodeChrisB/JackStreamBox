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

        public static void SetWindow(string windowName)
        {
            WindowName = windowName;
            Process[] ps = Process.GetProcessesByName(windowName);

            GameProcess = ps.FirstOrDefault();
        }

        public static bool SendInput(String input)
        {
            
            if (GameProcess == null) return false;

            //bring the window to the foreground
            IntPtr h = GameProcess.MainWindowHandle;
            SetForegroundWindow(h);

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
    }
}
