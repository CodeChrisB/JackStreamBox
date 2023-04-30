using JackBoxStream.Util.data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JackBoxStream.Util.logic
{
    internal class WindowNavigator
    {
        [DllImport("User32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr point);

        private string WindowName {get;set;}
        private Process GameProcess { get;set;}

        public WindowNavigator(string windowName)
        {
            WindowName = windowName;
            Process[] ps = Process.GetProcessesByName(windowName);

            GameProcess = ps.FirstOrDefault();
        }

        public bool SendInput(String input)
        {
            
            if (GameProcess == null) return false;

            //bring the window to the foreground
            IntPtr h = GameProcess.MainWindowHandle;
            SetForegroundWindow(h);

            SendKeys.SendWait(input);
            return true;
        }


    }
}
