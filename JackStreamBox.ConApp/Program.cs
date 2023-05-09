using JackStreamBox.Util;
using JackStreamBox.Util.Data;
namespace JackStreamBox.ConApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pick a Game");
            Console.WriteLine("[1] Ydkj2015\n[2] Fibbagexl\n[3] Drawful\n[4] Wordspud\n[5] Lieswatter\n[6] Fibbage2\n[7] Earwax\n[8] Bidiots\n[9] Quipplashxl\n[10] Bombcorp\n[11]  Quipplash2\n[12]  Triviamurderparty\n[13]  Guesspionage\n[14]  Teeko\n[15]  Fakeinit\n[16]  Fibbage3\n[17]  Surivetheinternet\n[18]  Monstermingle\n[19]  Bracketeering\n[20]  Civic\n[21]  Ydkj2018\n[22]  Splittheroom\n[23]  Madversecity\n[24]  Patentlystupid\n[25]  Zeepledoome\n[26]  Triviamurderparty2\n[27]  Dictionarium\n[28]  Pushthebutton\n[29]  Jokeboat\n[30]  Rolemodels\n[31]  Quipplash3\n[32]  Devilsandthedetails\n[33]  Champedup\n[34]  Talkingpoints\n[35]  Blatherround\n[36]  DrawfulAnimate\n[37]  WheelOfEnormousProportions\n[38]  Jobjob\n[39]  Pollmine\n[40]  WeaponsDrawn\n[41]  Fibbage4\n[42]  Quixort\n[43]  Junktopia\n[44]  Nonesensory\n[45]  Roomerang\n");
            Console.Write("Game Id: ");
            int gameNum =Int32.Parse(Console.ReadLine())-1;
            Game game = (Game)gameNum;


            Console.WriteLine("JackStreamBox");
            Console.WriteLine("Waiting 20 Second to open the game");
            //var task = JackStreamBoxUtility.OpenGame(game);
            //Console.WriteLine($"Result: {task.Result}");

            Task.Delay(3000);
            Console.WriteLine("Closing Game Game");
            //JackStreamBoxUtility.CloseGame();
        }

    }
}