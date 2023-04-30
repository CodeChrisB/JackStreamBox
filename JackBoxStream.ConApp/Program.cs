using JackBoxStream.Util;

namespace JackBoxStream.ConApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("JackStreamBox");
            var task = JackBoxStreamUtility.OpenGame(Util.data.Game.Bidiots);

            Console.WriteLine($"Result: {task.Result}");
        }
    }
}