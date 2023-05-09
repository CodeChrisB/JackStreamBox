using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Data
{
    public class CommandInfo
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public PermissionRole Role { get; private set; }

        public CommandInfo(string name, string description, PermissionRole role)
        {
            Name = name;
            Description = description;
            Role = role;
        }
    }
}
