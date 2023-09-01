using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Util.Data;

namespace JackStreamBox.Bot.Logic.Data
{
    public struct PackGame
    {
        public string Name;
        public string Description;
        public Game Id;

        public override string ToString()
        {
            return Name;
        }
    }
}
