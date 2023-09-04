using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand.ListSettings
{
    internal class ListLogic
    {
        private static string T(string file)
        {
            if (file == ListSerializer.BANNER) return "Banner";
            if (file == ListSerializer.RULE) return "Rule";
            return "";
        }

        public static async Task Add(CommandContext context,string file, string url)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;

            ListSerializer.AddEntry(file, url);
            await context.Channel.SendMessageAsync($"Added {T(file)}");
        }

        public static async Task Remove(CommandContext context,string file, int index)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;

            bool removed = ListSerializer.RemoveEntry(file, index);
            string text = removed ? $"Removed {T(file)}" : $"There was no index {index}";
            await context.Channel.SendMessageAsync(text);
        }

        public static async Task GetAll(CommandContext context,string file)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (string entry in ListSerializer.GetList(file))
            {
                sb.AppendLine($"{index}: {entry}");
                index++;

            }

            await context.Channel.SendMessageAsync(sb.ToString());

        }
    }
}
