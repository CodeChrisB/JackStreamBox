using JacktStreamBox.Database.Context;
using JacktStreamBox.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacktStreamBox.Database.Logic
{
    public class PlayerLogic
    {
        private readonly BotDbContext _dbContext;

        public PlayerLogic(BotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Player CreatePlayer(ulong id)
        {
            var player = new Player
            {
                Id = id,
                PlayXP = 0
            };

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
            return player;
        }

        public Player? GetPlayerById(ulong id)
        {
            return _dbContext.Players.FirstOrDefault(p => p.Id == id);
        }

        public void AddXpToPlayer(ulong id, ulong xpToAdd)
        {
            var player = GetPlayerById(id);
            if (player != null)
            {
                player.PlayXP += xpToAdd;
                _dbContext.SaveChanges();
            }
        }

        public List<Player> GetTopPlayersByXp(int n)
        {
            return _dbContext.Players.OrderByDescending(p => p.PlayXP).Take(n).ToList();
        }

        public int GetTotalPlayerCount()
        {
            return _dbContext.Players.Count();
        }
    }
}
