using JackBoxStream.Util;
using JackBoxStream.Util.data;

namespace JackBoxStream.ConApp
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            Console.WriteLine("JackStreamBox");
            var task = JackBoxStreamUtility.OpenGame(Game.Bidiots);
            await task;

            Console.WriteLine($"Result: {task.Result}");
        }
    }
}