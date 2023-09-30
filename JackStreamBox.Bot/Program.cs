using JackStreamBox.Bot.Logic.Config;
using System.Diagnostics;
using System.Reflection;

namespace JackStreamBox.Bot
{
    internal class Program
    {
        private static Bot Bot { get; set; }
        static async Task Main(string[] args)
        {

            // Set up the global exception handler
            AppDomain.CurrentDomain.UnhandledException += OnCrash;

            try
            {
                Bot = new Bot();
                await Bot.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine($"Crash - Bot crashed lol");

                var channel = await Bot.Client.GetChannelAsync(ChannelId.LogChannel);
                if(channel != null )
                {
                    await channel.SendMessageAsync(ex.Message);
                }
            }

        }

        static async void OnCrash(object sender, UnhandledExceptionEventArgs e)
        {

			var channel = await Bot.Client.GetChannelAsync(ChannelId.LogChannel);
			if (channel != null)
			{
				await channel.SendMessageAsync(e.ToString());
			}
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