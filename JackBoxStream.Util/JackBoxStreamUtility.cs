using JackBoxStream.Util.data;
using JackBoxStream.Util.logic;

namespace JackBoxStream.Util
{
    public class JackBoxStreamUtility
    {
        /// <summary>
        /// The public function to open a specific
        /// </summary>
        /// <param name="game"></param> An enum value indicating which game should be opend
        /// <returns></returns>
        /// A bool value indicating the sucess of the
        public static async Task<bool> OpenGame(Game game)
        {
            GameOpener opener = new GameOpener();

            var task = opener.Open(Game.Bidiots);
            await task;
            return task.Result;

            //D:\SteamLibrary\steamapps\common\The Jackbox Party Pack 
        }
    }
}