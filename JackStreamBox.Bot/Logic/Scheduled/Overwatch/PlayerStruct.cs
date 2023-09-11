using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Scheduled.Overwatch
{
    internal struct Player
    {
        public Player(ulong id, ulong hostXP, ulong playXP)
        {
            Id = id;
            HostXP = hostXP;
            PlayXP = playXP;
        }

        public ulong Id { get; set; }
        public ulong HostXP { get; set; }
        public ulong PlayXP { get; set; }



        public override string ToString()
        {
            return $"{Id},{HostXP},{PlayXP}";
        }
    }
}
