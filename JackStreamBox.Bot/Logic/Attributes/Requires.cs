using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Attributes
{
    internal class Requires : Attribute
    {
        public PermissionRole RequiredLevel { get; }

        public Requires(PermissionRole level)
        {
            this.RequiredLevel = level;
        }
    }
}
