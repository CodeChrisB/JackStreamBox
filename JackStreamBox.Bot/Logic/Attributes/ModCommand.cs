using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Attributes
{
    internal class ModCommand : Attribute
    {
        public PermissionRole RequiredLevel { get; }
        public ModCommand(PermissionRole level)
        {
            this.RequiredLevel = level;
        }
    }
}
