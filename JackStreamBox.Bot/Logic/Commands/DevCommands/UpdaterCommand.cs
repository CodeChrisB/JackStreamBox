using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JackStreamBox.Bot.Logic.Config;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace JackStreamBox.Bot.Logic.Commands.DevCommands
{
    internal class UpdaterCommand : BaseCommandModule
    {

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();


        [Command("update")]
        [Description("Update the bot")]
        [ModCommand(PermissionRole.DEVELOPER)]
        public async Task Update(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;

            await context.Channel.SendMessageAsync("Aight, getting newest version");
            // Get the current directory of the application
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            // Get the root folder path by going up one directory level
            string rootFolderPath = Directory.GetParent(currentDirectory).FullName;
            string? projectFolder = null;
            if (!string.IsNullOrEmpty(rootFolderPath))
            {
                int pathNum = rootFolderPath.Split("\\").Length - 3;
                projectFolder = string.Join("\\", rootFolderPath.Split("\\").ToList().Take(pathNum).ToArray());
            }

            Process.Start($"{projectFolder}\\updater.bat");
            await context.Channel.SendMessageAsync("See ya in a minute when I restart");
            BotData.IncrementValue("message",2);
            Environment.Exit(1);
        }


        [Command("restart")]
        [Description("Restart the bot")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task RestartBot(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            await context.Channel.SendMessageAsync("Aight, restarting the bot");
            // Get the current directory of the application
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            // Get the root folder path by going up one directory level
            string rootFolderPath = Directory.GetParent(currentDirectory).FullName;
            string? projectFolder = null;
            if (!string.IsNullOrEmpty(rootFolderPath))
            {
                int pathNum = rootFolderPath.Split("\\").Length - 3;
                projectFolder = string.Join("\\", rootFolderPath.Split("\\").ToList().Take(pathNum).ToArray());
            }

            Process.Start($"{projectFolder}\\restarter.bat");
            await context.Channel.SendMessageAsync("See ya in a few seconds when I restart");
            BotData.IncrementValue("message", 2);
            Environment.Exit(1);
        }

        [Command("version")]
        [Description("Check bot version")]
        [Requires(PermissionRole.TRUSTED)]
        public async Task Utest(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED)) return;
            await context.Channel.SendMessageAsync(GenerateVersionString());
            

        }

        private static string GenerateVersionString()
        {
            DateTime fixedDate = new DateTime(2023, 7, 1);
            DateTime currentDate = DateTime.Now;

            int monthsDifference = (currentDate.Year - fixedDate.Year) * 12 + currentDate.Month - fixedDate.Month;
            int daysDifference = (int)(currentDate - fixedDate).TotalDays;

            int x = monthsDifference + 1;
            int y = daysDifference % 10 + 1;

            return $"v1.{x}.{y}";
        }



    }
}
