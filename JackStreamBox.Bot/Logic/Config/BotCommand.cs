using JackStreamBox.Bot.Logic.Attributes;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JackStreamBox.Bot.Logic.Config
{
    public static class BotCommand
    {
        private static List<CommandInfo> UserCommands = new List<CommandInfo>();
        private static List<CommandInfo> StaffCommands = new List<CommandInfo>();
        private static List<CommandInfo> DevCommands = new List<CommandInfo>();

        public static CommandInfo[] GetUserCommands() => UserCommands.OrderBy(x => (int)x.Role).ThenBy(x => x.Name).ToArray();
        public static CommandInfo[] GetStaffCommands()  => StaffCommands.ToArray();
        public static CommandInfo[] GetDeveloperCommands() => DevCommands.ToArray();

        public static void Register<T>()
        {
            var type = typeof(T);


            var methods = type.GetMethods();


            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(true);
                // Do something with the attributes

                foreach (Attribute attr in attributes)
                {
                    if (attr.TypeId == typeof(Requires))
                    {
                        //We found a user command
                        AppendUserCommand(attributes);
                    }

                    if(attr.TypeId == typeof(ModCommand))
                    {
                        //We found a staff/dev command
                        AppendStaffCommand(attributes);
                    }
                }
            }
        }

        public static async Task GenerateMarkdown()
        {
            await DocGenerator.GenerateMarkdown(UserCommands.Concat(StaffCommands).Concat(DevCommands).ToArray());
        }

        private static void AppendStaffCommand(object[] attributes)
        {
            string name = String.Empty;
            string description = String.Empty;
            string emoji = String.Empty;
            PermissionRole role = PermissionRole.ANYONE;
            foreach (Attribute attr in attributes)
            {
                if (attr.TypeId == typeof(ModCommand))
                {
                    //We found a method that has an required Level Information save its data
                    role = ((ModCommand)attr).RequiredLevel;
                }
                else if (attr.TypeId == typeof(CoammandDescription))
                {
                    description = ((CoammandDescription)attr).Description;
                    emoji = ((CoammandDescription)attr).Emoji;
                }
                else if (attr.TypeId == typeof(CommandAttribute))
                {
                    name = ((CommandAttribute)attr).Name;
                }
            }
            CommandInfo ci = new CommandInfo(name,emoji, description, role);
            if(role == PermissionRole.DEVELOPER) {
                DevCommands.Add(ci);
            }
            else
            {
                StaffCommands.Add(ci);
            }
        }
        private static void AppendUserCommand(object[] attributes)
        {
            string name = String.Empty;
            string description = String.Empty;
            string emoji = String.Empty;
            PermissionRole role = PermissionRole.ANYONE;
            foreach (Attribute attr in attributes)
            {
                if (attr.TypeId == typeof(Requires))
                {
                    //We found a method that has an required Level Information save its data
                    role = ((Requires)attr).RequiredLevel;
                }
                else if(attr.TypeId == typeof(CoammandDescription))
                {
                    description = ((CoammandDescription)attr).Description;
                    emoji = ((CoammandDescription)attr).Emoji;
                }
                else if(attr.TypeId == typeof(CommandAttribute))
                {
                    name = ((CommandAttribute)attr).Name;
                }
           }
            CommandInfo ci = new CommandInfo(name, emoji, description,role);
            UserCommands.Add(ci);
        }
    }
}
