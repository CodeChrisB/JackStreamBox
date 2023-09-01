using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JackStreamBox.Bot.Logic.Attributes
{
    internal class CoammandDescription : Attribute
    {
        public string Description { get; }
        public string Emoji { get; }

        public CoammandDescription(string description,string emoji)
        {
            this.Description = description;
            this.Emoji = emoji; 
        }
    }
}
