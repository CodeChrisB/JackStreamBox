using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Data
{
    public class Joke
    {
        public static string GetJoke()
        {
            string path =$"{Directory.GetCurrentDirectory()}\\Logic\\Data\\Jokes.txt";
            string[] jokes = File.ReadAllLines(path);
            Random random = new Random();
            return jokes[random.Next(jokes.Length-1)];
        }
    }
}
