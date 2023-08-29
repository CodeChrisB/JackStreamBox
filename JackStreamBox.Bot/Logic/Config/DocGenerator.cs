using JackStreamBox.Bot.Logic.Data;

using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Config
{
    internal class DocGenerator
    {

        public static string PASTE_BIN_KEY ="";
        public static string PASTE_BIN_URL = "";
        public static async Task GenerateMarkdown(CommandInfo[] commands)
        {
            StringBuilder sb = new StringBuilder();

            Header(sb);
            Commands(sb, commands);



            if (HasNewContent(sb.ToString()))
            {
                //string updatedUrl = await UploadToPastebin(PASTE_BIN_KEY, sb.ToString());
                //PASTE_BIN_URL = ConvertToRawUrl(updatedUrl);
                WriteAll(sb); 
                Console.WriteLine($"Generator - Updates found using Reflection, new markdown for github and pastebin will be generated!");
                Console.WriteLine("Generator - Done");
            }
            else
            {
                Console.WriteLine("Generator - No Update for Commands found using Reflection, no new markdown will be generated");
            }
 
        }

        static string ConvertToRawUrl(string pastebinUrl)
        {
            if (pastebinUrl == null) return null;
            Uri uri = new Uri(pastebinUrl);
            string pasteId = uri.Segments[2]; // Get the segment after "pastebin.com/"

            string rawUrl = $"https://pastebin.com/raw/{pasteId}";
            return rawUrl;
        }
        private static void Commands(StringBuilder sb, CommandInfo[] commands)
        {
            int currentlevel = 0;
            foreach (var command in commands.OrderBy(x => (int)x.Role).ThenBy(x => x.Name))
            {


                if (currentlevel < (int)command.Role)
                {

                    currentlevel = (int)command.Role;
                    sb.AppendLine($"## Level {currentlevel}  {CommandLevel.RoleName(command.Role)} ");
                    sb.AppendLine("| Command      | Description        |");
                    sb.AppendLine("|--------------|--------------------|");

                }

                sb.AppendLine($"| **!{command.Name}**| {command.Description}|");
            }
        }

        private static void Header(StringBuilder sb)
        {

            sb.AppendLine("![image](https://user-images.githubusercontent.com/55576076/235742815-f471e12a-7e11-45ee-aad4-25b1b0aa38ab.png");
            sb.AppendLine("### A 24/7 Jackbox Party Pack Bot.  ");
            sb.AppendLine("## How does it work  ");
            sb.AppendLine("The bot automatically opens the games so that players can join.");
            sb.AppendLine("After a game players can vote for the next game, after the voting the next game gets opend.");

        }

        private static string path()
        {
            string[] rootPath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");  // Get the root folder of the solution
            string path = string.Join("\\", rootPath, 0, rootPath.Length - 5);
            return Path.Combine(path, "readme.md");  // Specify the file path in the root folder
        }

        private static void WriteAll(StringBuilder sb)
        {
            string filePath = path();
            Console.WriteLine(filePath);
            try
            {
                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine("File created or overwritten successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task<string> UploadToPastebin(string apiKey, string content)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "https://pastebin.com/api/api_post.php";

                var parameters = new
                {
                    api_dev_key = apiKey,
                    api_option = "paste",
                    api_paste_code = content
                };

                var contentData = new MultipartFormDataContent();
                foreach (var param in parameters.GetType().GetProperties())
                {
                    contentData.Add(new StringContent(param.GetValue(parameters)?.ToString()), param.Name);
                }

                var response = await httpClient.PostAsync(apiUrl, contentData);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (responseContent.StartsWith("https://pastebin.com/"))
                    {
                        return responseContent.Trim(); // This should be the Pastebin URL
                    }
                    else
                    {
                        Console.WriteLine("Error uploading to Pastebin: " + responseContent);
                    }
                }
                else
                {
                    Console.WriteLine("Error uploading to Pastebin: " + response.ReasonPhrase);
                }
            }

            return null;
        }

        static bool HasNewContent(string stringBuilderContent)
        {
            string filePath = path();
            try
            {
                string fileContent = File.ReadAllText(filePath);
                return fileContent != stringBuilderContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
