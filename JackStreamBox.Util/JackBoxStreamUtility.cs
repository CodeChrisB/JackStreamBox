using JackStreamBox.Util.Data;
using JackStreamBox.Util.logic;
using System;
using System.Threading.Tasks;

namespace JackStreamBox.Util
{
    public class JackStreamBoxUtility
    {

        /// <summary>
        /// The public function to open a specificm game
        /// </summary>
        /// <param name="game"></param> An enum value indicating which game should be opend
        /// <returns></returns>
        /// A bool value indicating the sucess of the
        public static async Task<bool> OpenGame(Game game)
        {
            var task = GameOpener.Open(game);
            await task;
            return task.Result;
        }


        /// <summary>
        /// The public function close the current game
        /// </summary>
        /// <param name="game"></param> An enum value indicating which game should be opend
        /// <returns></returns>
        /// A bool value indicating the sucess of the
        public static bool CloseGame() 
        {
            return WindowNavigator.Close();
        }

        /// <summary>
        /// Used to set the steampath the tool uses to open the games.
        /// </summary>
        /// <param name="path"></param> The Given Steam Path
        /// <exception cref="NotImplementedException"></exception>
        public static void SetSteamPath(string path)
        {
            throw new NotImplementedException();
        }



    }
}