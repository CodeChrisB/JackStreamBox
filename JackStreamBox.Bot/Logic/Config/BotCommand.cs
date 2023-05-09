using JackStreamBox.Bot.Logic.Attributes;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Config
{
    public static class BotCommand
    {
        private static List<CommandInfo> CommandInfoList = new List<CommandInfo>();


        public static CommandInfo[] GetCommands()
        {
            return CommandInfoList.OrderBy(x => x.Name).ToArray();
        }
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
                        //We found a method that has an required Level Information save its data
                        AppendAttribute(attributes);
                    }
                }
            }
        }

        private static void AppendAttribute(object[] attributes)
        {
            string name = String.Empty;
            string description = String.Empty;
            PermissionRole role = PermissionRole.DEVELOPER;
            foreach (Attribute attr in attributes)
            {
                if (attr.TypeId == typeof(Requires))
                {
                    //We found a method that has an required Level Information save its data
                    role = ((Requires)attr).RequiredLevel;
                }
                else if(attr.TypeId == typeof(DescriptionAttribute))
                {
                    description = ((DescriptionAttribute)attr).Description;
                }
                else if(attr.TypeId == typeof(CommandAttribute))
                {
                    name = ((CommandAttribute)attr).Name;
                }
           }
            CommandInfo ci = new CommandInfo(name, description,role);
            CommandInfoList.Add(ci);
        }
    }
}
