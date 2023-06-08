using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Util;
using JackStreamBox.Util.Data;

namespace JackStreamBox.Bot.Logic.Data
{
    public struct Pack
    {
        public string Name;
        public PackGame[] games;
    }
    
    public class PackInfo
    {
        public static Pack GetPackInfo(int pack)
        {
            return AllPacks()[pack-1];


        }

        public static Pack[] GetAllPacks() { return AllPacks(); }

        public static PackGame[] GetAllGames()
        {
            List<PackGame> games = new List<PackGame>();
            foreach (Pack pack in PackInfo.GetAllPacks())
            {
                games.AddRange(pack.games);
            }
            return games.ToArray();
        }

        public static PackGame[] GetRandomGames(int amount)
        {
            PackGame[] games = GetAllGames();
            PackGame[] result = new PackGame[amount];

            for(int i=0; i<amount; i++)
            {
                result[i] = games[new Random().Next(games.Length + 1)];
            }

            result = result.Distinct().ToArray();
            if (result.Length < amount) result = GetRandomGames(amount);
            return result;


        }

        private static Pack[] AllPacks()
        {
            return new Pack[]
                {
                    new Pack
                    {
                        Name = "Jackbox Party Pack 1",
                        games = new PackGame[]
                        {
                            new PackGame {Id= Game.Ydkj2015, Name = "You Don't Know Jack", Description = "A trivia game where players answer questions to score points." },
                            new PackGame {Id= Game.Fibbagexl, Name = "Fibbage XL", Description = "A bluffing game where players make up fake answers to real trivia questions." },
                            new PackGame {Id= Game.Drawful, Name = "Drawful", Description = "A drawing game where players try to guess what each other are drawing." },
                            new PackGame {Id= Game.Wordspud, Name = "Word Spud", Description = "A word game where players take turns creating a chain of words." },
                            new PackGame {Id = Game.Lieswatter,  Name = "Lie Swatter", Description = "A trivia game where players have to determine which statements are true and which are lies." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 2",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Quipplashxl,  Name = "Quiplash XL", Description = "A game where players answer prompts with witty responses to try and win votes." },
                            new PackGame {Id = Game.Fibbage2,  Name = "Fibbage 2", Description = "A sequel to Fibbage with new questions and features." },
                            new PackGame {Id = Game.Earwax,  Name = "Earwax", Description = "A sound-effects game where players create funny sound combinations." },
                            new PackGame {Id = Game.Bidiots,  Name = "Bidiots", Description = "A drawing game where players draw and auction off their creations." },
                            new PackGame {Id = Game.Bombcorp,  Name = "Bomb Corp.", Description = "A cooperative game where players work together to defuse bombs." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 3",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Quipplash2, Name = "Quiplash 2", Description = "A sequel to Quiplash with new prompts and features." },
                            new PackGame {Id = Game.Triviamurderparty, Name = "Trivia Murder Party", Description = "A horror-themed trivia game where players try to avoid being killed." },
                            new PackGame {Id = Game.Guesspionage, Name = "Guesspionage", Description = "A game where players try to guess how the majority of people answered survey questions." },
                            new PackGame {Id = Game.Teeko, Name = "Tee K.O.", Description = "A drawing game where players create t-shirt designs and slogans." },
                            new PackGame {Id = Game.Fakeinit, Name = "Fakin' It", Description = "A bluffing game where players try to hide the fact that they don't know the answer." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 4",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Fibbage3, Name = "Fibbage 3", Description = "A sequel to Fibbage with new questions and features." },
                            new PackGame {Id = Game.Surivetheinternet, Name = "Survive the Internet", Description = "A game where players take each other's responses out of context to make them look bad." },
                            new PackGame {Id = Game.Monstermingle, Name = "Monster Seeking Monster", Description = "A dating game where players try to date and kill each other." },
                            new PackGame {Id = Game.Bracketeering, Name = "Bracketeering", Description = "A game where players predict the outcome of ridiculous matchups." },
                            new PackGame {Id = Game.Civic, Name = "Civic Doodle", Description = "A drawing game where players draw and improve on each other's town murals." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 5",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Ydkj2018, Name = "You Don't Know Jack: Full Stream", Description = "A trivia game where players answer questions to score points." },
                            new PackGame {Id = Game.Splittheroom,  Name = "Split the Room", Description = "A game where players create divisive prompts to try and split the group's opinion." },
                            new PackGame {Id = Game.Madversecity,  Name = "Mad Verse City", Description = "A rap battle game where players write rap verses to battle each other." },
                            new PackGame {Id = Game.Patentlystupid,  Name = "Patently Stupid", Description = "A game where players invent and pitch ridiculous inventions to solve problems." },
                            new PackGame {Id = Game.Zeepledoome,  Name = "Zeeple Dome", Description = "A game where players fight aliens in an arena by launching themselves at them." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 6",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Triviamurderparty2, Name = "Trivia Murder Party 2", Description = "A sequel to Trivia Murder Party with new questions and features." },
                            new PackGame {Id = Game.Dictionarium, Name = "Dictionarium", Description = "A game where players invent definitions for made-up words and vote on their favorites." },
                            new PackGame {Id = Game.Pushthebutton, Name = "Push the Button", Description = "A game where players try to determine who among them are aliens." },
                            new PackGame {Id = Game.Jokeboat, Name = "Joke Boat", Description = "A game where players write and perform their own stand-up comedy routines." },
                            new PackGame {Id = Game.Rolemodels,  Name = "Role Models", Description = "A game where players try to match each other up with the most fitting personas." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 7",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Quipplash3, Name = "Quiplash 3", Description = "A sequel to Quiplash with new prompts and features." },
                            new PackGame {Id = Game.Devilsandthedetails, Name = "The Devils and the Details", Description = "A cooperative game where players play as a family of devils trying to accomplish tasks in the mortal world." },
                            new PackGame {Id = Game.Champedup, Name = "Champ'd Up", Description = "A drawing game where players create their own fighters to compete in a championship tournament." },
                            new PackGame {Id = Game.Talkingpoints, Name = "Talking Points", Description = "A game where players give presentations using slides they haven't seen before." },
                            new PackGame {Id = Game.Blatherround, Name = "Blather 'Round", Description = "A game where players try to get others to guess a secret phrase without using any of the forbidden words." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 8",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.DrawfulAnimate, Name = "Drawful Animate", Description = "Drawful but now with Animation !" },
                            new PackGame {Id = Game.WheelOfEnormousProportions, Name = "The Wheel of Enourmous Proportions", Description = "Answer Trivia Question and get lucky to win the wheel !" },
                            new PackGame {Id = Game.Jobjob,  Name = "Job Job", Description = "Answer Questions. Win a Job." },
                            new PackGame {Id = Game.Pollmine,  Name = "The Poll Mine", Description = "Answer Questions, Pick Doors, maybe surive." },
                            new PackGame {Id = Game.WeaponsDrawn,  Name = "Weapons Drawn", Description = "Hide a letter in a murder weapon you design, the others will try to catch you." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 9",
                        games = new PackGame[]
                        {
                            new PackGame {Id = Game.Fibbage4, Name = "Fibbage 4", Description = "A sequel of the sequel to Fibbage with new questions and features." },
                            new PackGame {Id = Game.Roomerang, Name = "Roomerang", Description = "Jackbox but it's a reality tv show" },
                            new PackGame {Id = Game.Junktopia,  Name = "Junktopia", Description = "Buy trash and make a funny backstory to sell it" },
                            new PackGame {Id = Game.Nonesensory,  Name = "Nonesensory", Description = "Rank things between weird scales like the scale between apple and monkey." },
                            new PackGame {Id = Game.Quixort,  Name = "Quixort", Description = "Order things, be better than the other team and win. It's so easy." }
                        }
                    },
                };
        }

        internal static PackGame[] GetVotePack(string vote)
        {
            if(vote.Length == 1)
            {
                int pack = Int32.Parse(vote);
                return AllPacks()[pack].games;
            }

            switch (vote)
            {
                case "draw":
                    return GenSelection(Game.Drawful, Game.DrawfulAnimate, Game.Teeko, Game.Champedup, Game.WeaponsDrawn);
                case "trivia":
                    return GenSelection(Game.Triviamurderparty2, Game.Fibbage3, Game.Fibbage4, Game.Quixort, Game.WheelOfEnormousProportions);
                case "fun":
                    return GenSelection(Game.Jobjob, Game.Earwax, Game.Roomerang, Game.Patentlystupid, Game.Surivetheinternet);
                case "talk":
                    return GenSelection(Game.Talkingpoints, Game.Patentlystupid, Game.Pushthebutton, Game.Blatherround, Game.Junktopia);
            }

            return AllPacks()[7].games;
        }

        public static string VoteCategories()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("╔════════════════════");
            sb.AppendLine("║You can vote for any pack itself using **!pack X**");
            sb.AppendLine("║e.g **!pack 5** or **!pack 8**");
            sb.AppendLine("╠═ Categories ══");
            sb.AppendLine("║**!vote draw** Will only pick drawing games");
            sb.AppendLine("║**!vote trivia** Will only pick triva games");
            sb.AppendLine("║**!vote fun** Will only pick 'fun' games");
            sb.AppendLine("║**!vote mic** Will pick games were a mic is required.");
            sb.AppendLine("╚════════════════════");


            return sb.ToString();
        }

        private static PackGame[] GenSelection(Game g1, Game g2, Game g3, Game g4, Game g5)
        {
            return new PackGame[]
            {
                GetAllGames()[(int)g1],
                GetAllGames()[(int)g2],
                GetAllGames()[(int)g3],
                GetAllGames()[(int)g4],
                GetAllGames()[(int)g5]
            };
        }
    }
}
