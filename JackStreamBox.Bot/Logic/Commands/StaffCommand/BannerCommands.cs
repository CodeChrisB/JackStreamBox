using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    internal class BannerCommands : BaseCommandModule
    {

        [Command("banner+")]
        [Description("Adds a banner shown while at multiple pages")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task AddBanner(CommandContext context, string url)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;

            CustomBanner.AddBanner(url);
            await context.Channel.SendMessageAsync("Added banner");
            
        }

        [Command("banner-")]
        [Description("Adds a banner shown while at multiple pages")]
        [ModCommand(PermissionRole.STAFF)]

        public async Task RemoveBanner(CommandContext context, int index)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;

            bool removed = CustomBanner.RemoveBanner(index);
            string text = removed ? "Removed banner" : $"There was no index {index}";
            await context.Channel.SendMessageAsync(text);
            


        }

        [Command("banners")]
        [Description("Adds a banner shown while at multiple pages")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task GetAllBanners(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (string banner in CustomBanner.GetAllBanner())
            {
                sb.AppendLine($"{index}: {banner}");
                index++;

            }

            await context.Channel.SendMessageAsync(sb.ToString());
            
        }
    }
}
