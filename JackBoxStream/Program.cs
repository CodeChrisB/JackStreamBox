using JackStreamBox.Util;
using JackStreamBox.Util.data;

namespace JackStreamBox.ConApp
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            Console.WriteLine("JackStreamBox");
            var task = JackStreamBoxUtility.OpenGame(Game.Bidiots);
            await task;

            Console.WriteLine($"Result: {task.Result}");
        }
    }
}