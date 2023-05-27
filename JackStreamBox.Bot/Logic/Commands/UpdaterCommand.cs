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

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class UpdaterCommand : BaseCommandModule
    {

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();


        [Command("update")]
        [Description("View the rules.")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task Update(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;

            // Execute "git stash && git pull" commands
            ExecuteShellCommand("git stash && git pull");

            RebuildApplication();
            // Restart the bot
            RestartBot();
        }

        [Command("utest")]
        [Description("Test updater")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task Utest(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
            await context.Channel.SendMessageAsync("V0.4.1");
        }

        static void ExecuteShellCommand(string command)
        {
            // Create a new process
            Process process = new Process();

            // Set the command and arguments
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command;

            // Redirect the output if necessary
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            // Enable shell execute
            process.StartInfo.UseShellExecute = false;

            // Start the process
            process.Start();

            // Read the output and error messages, if needed
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            // Wait for the process to finish
            process.WaitForExit();

            // Display the output and errors
            Console.WriteLine("Output: " + output);
            Console.WriteLine("Errors: " + errors);
        }

        static void RebuildApplication()
        {
            Console.WriteLine("Rebuilding the application...");

            // Get the current working directory
            string currentDirectory = Environment.CurrentDirectory;

            // Search for the project file within the current directory and its subdirectories
            string projectFilePath = SearchForProjectFile(currentDirectory);

            if (projectFilePath != null)
            {
                // Rebuild the project using dotnet build command
                ExecuteShellCommand("dotnet build \"" + projectFilePath + "\"");
            }
            else
            {
                Console.WriteLine("No project file found in the current directory and its subdirectories.");
            }
        }

        static string SearchForProjectFile(string directory)
        {
            // Search for files with .csproj extension within the directory
            var projectFiles = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories);

            // Return the first project file found, if any
            return projectFiles.FirstOrDefault();
        }

        static void RestartBot()
        {
            Console.WriteLine("Bot is restarting...");

            FreeConsole();

            // Get the path of the current executable
            string appPath = Process.GetCurrentProcess().MainModule.FileName;

            // Start a new instance of the application
            Process.Start(new ProcessStartInfo
            {
                FileName = appPath,
                UseShellExecute = true
            });

            // Exit the current instance
            Environment.Exit(0);
        }
    }
}
