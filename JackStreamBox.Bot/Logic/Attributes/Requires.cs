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
        //
        // Summary:
        //     Marks this command to be listed inside the help page. The command will only show up if the user has the required permission level.
        //     help.
        //
        // Parameters:
        //   level:
        public Requires(PermissionRole level)
        {
            this.RequiredLevel = level;
        }
    }
}
