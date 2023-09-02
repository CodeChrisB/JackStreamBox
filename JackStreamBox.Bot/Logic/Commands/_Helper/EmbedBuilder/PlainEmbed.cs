using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder
{
    internal class PlainEmbed
    {
        public static FluentBuilder CreateEmbed(CommandContext context)
        {
            return  new FluentBuilder(context);
        }
        public static FluentBuilder CreateEmbed(CustomContext context)
        {
            return new FluentBuilder(context);
        }
        public static FluentBuilder CreateEmbed()
        {
            return new FluentBuilder();
        }

        public static  async Task<DiscordMessage> Build(CommandContext context, DiscordEmbedBuilder embed)
        {
            return await context.Channel.SendMessageAsync(embed: embed.Build());
        }

        public static async void BuildNDestroy(CommandContext context, DiscordEmbedBuilder embed , TimeSpan destroyTime)
        {
            Destroyer.Message(await Build(context, embed), destroyTime);
        }
    }
}


public class FluentBuilder
{
    DiscordEmbedBuilder builder;
    CustomContext _context;    
    
    public FluentBuilder(CommandContext context)
    {
        _context = new CustomContext(context);
        builder = new DiscordEmbedBuilder();
    }
    public FluentBuilder(CustomContext context)
    {
        _context = context;
        builder = new DiscordEmbedBuilder();
    }

    public FluentBuilder()
    {
        builder = new DiscordEmbedBuilder();
    }

    //Chainable Commands
    public FluentBuilder Title(string title)
    {
        builder.Title = title;
        return this;
    }
    public FluentBuilder Description(string description)
    {
        builder.Description = description;
        return this;
    }
    public FluentBuilder Color(DiscordColor color)
    {
        builder.Color = color;
        return this;
    }
    public FluentBuilder DescriptionAddLine(string description)
    {
        builder.Description += "\n"+description;
        return this;
    }
    public FluentBuilder ImageUrl(string url)
    {
        builder.ImageUrl = url;
        return this;
    }


    //End Commands
    public DiscordEmbedBuilder GetBuilder() => builder;

    public async Task<DiscordMessage> Build()
    {
        return await _context.Channel.SendMessageAsync(embed: builder.Build());
    }

    public async Task BuildNDestroy(TimeSpan destroyTime)
    {
        Destroyer.Message(await Build(),destroyTime);
    }
}

