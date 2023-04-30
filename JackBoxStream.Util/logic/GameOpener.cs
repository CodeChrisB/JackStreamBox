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
            string path = "D:\\SteamLibrary\\steamapps\\common\\The Jackbox Party Pack 8\\The Jackbox Party Pack 8.exe";
            
            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;

            try
            {
                // Start the process
                Process process = Process.Start(startInfo);

                // Wait until the program is fully launched
                if(process == null) return false;
                await Task.Delay(Time.SECOND * 1);

                // Return true if the process was started successfully
                return true;
            }
            catch (Exception ex)
            {
                // An exception occurred, return false
                Console.WriteLine("Failed to start process: " + ex.Message);
                return false;
            }
        }
    }
}
