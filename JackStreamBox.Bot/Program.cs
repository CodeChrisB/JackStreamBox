using JackStreamBox.Bot.Logic.Config;
using System.Diagnostics;
using System.Reflection;

namespace JackStreamBox.Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            // Set up the global exception handler
            AppDomain.CurrentDomain.UnhandledException += OnCrash;


            while (true) // Infinite loop for restarting the bot
            {
                try
                {
                    var bot = new Bot();
                    await bot.RunAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Crash - Bot crashed lol");
                    //DocGenerator.WriteLog(ex.Message);
                }
            }
        }

        static void OnCrash(object sender, UnhandledExceptionEventArgs e)
        {
            // Get the current directory of the application
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            // Get the root folder path by going up one directory level
            string rootFolderPath = Directory.GetParent(currentDirectory).FullName;
            string? projectFolder = null;
            if (!string.IsNullOrEmpty(rootFolderPath))
            {
                int pathNum = rootFolderPath.Split("\\").Length - 3;
                projectFolder = string.Join("\\", rootFolderPath.Split("\\").ToList().Take(pathNum).ToArray());
            }

            Process.Start($"{projectFolder}\\restarter.bat");
            BotData.IncrementValue("message", 2);
            Environment.Exit(1);
        }
    }
}