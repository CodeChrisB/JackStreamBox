using JackStreamBox.Bot.Logic.Config;

namespace JackStreamBox.Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
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
                    DocGenerator.WriteLog(ex.Message);
                    Bot.CRASHED = true;
                }
            }
        }
    }
}