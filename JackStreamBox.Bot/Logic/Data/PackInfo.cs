using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Data
{
    public struct Pack
    {
        public string Name;
        public Game[] games;
    }



    
    public class PackInfo
    {
        public static Pack GetPackInfo(int pack)
        {
            return AllPacks()[pack-1];


        }


        private static Pack[] AllPacks()
        {
            return new Pack[]
                {
                    new Pack
                    {
                        Name = "Jackbox Party Pack 1",
                        games = new Game[]
                        {
                            new Game { Name = "You Don't Know Jack", Description = "A trivia game where players answer questions to score points." },
                            new Game { Name = "Fibbage XL", Description = "A bluffing game where players make up fake answers to real trivia questions." },
                            new Game { Name = "Drawful", Description = "A drawing game where players try to guess what each other are drawing." },
                            new Game { Name = "Word Spud", Description = "A word game where players take turns creating a chain of words." },
                            new Game { Name = "Lie Swatter", Description = "A trivia game where players have to determine which statements are true and which are lies." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 2",
                        games = new Game[]
                        {
                            new Game { Name = "Quiplash XL", Description = "A game where players answer prompts with witty responses to try and win votes." },
                            new Game { Name = "Fibbage 2", Description = "A sequel to Fibbage with new questions and features." },
                            new Game { Name = "Earwax", Description = "A sound-effects game where players create funny sound combinations." },
                            new Game { Name = "Bidiots", Description = "A drawing game where players draw and auction off their creations." },
                            new Game { Name = "Bomb Corp.", Description = "A cooperative game where players work together to defuse bombs." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 3",
                        games = new Game[]
                        {
                            new Game { Name = "Quiplash 2", Description = "A sequel to Quiplash with new prompts and features." },
                            new Game { Name = "Trivia Murder Party", Description = "A horror-themed trivia game where players try to avoid being killed." },
                            new Game { Name = "Guesspionage", Description = "A game where players try to guess how the majority of people answered survey questions." },
                            new Game { Name = "Tee K.O.", Description = "A drawing game where players create t-shirt designs and slogans." },
                            new Game { Name = "Fakin' It", Description = "A bluffing game where players try to hide the fact that they don't know the answer." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 4",
                        games = new Game[]
                        {
                            new Game { Name = "Fibbage 3", Description = "A sequel to Fibbage with new questions and features." },
                            new Game { Name = "Survive the Internet", Description = "A game where players take each other's responses out of context to make them look bad." },
                            new Game { Name = "Monster Seeking Monster", Description = "A dating game where players try to date and kill each other." },
                            new Game { Name = "Bracketeering", Description = "A game where players predict the outcome of ridiculous matchups." },
                            new Game { Name = "Civic Doodle", Description = "A drawing game where players draw and improve on each other's town murals." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 5",
                        games = new Game[]
                        {
                            new Game { Name = "You Don't Know Jack: Full Stream", Description = "A trivia game where players answer questions to score points." },
                            new Game { Name = "Split the Room", Description = "A game where players create divisive prompts to try and split the group's opinion." },
                            new Game { Name = "Mad Verse City", Description = "A rap battle game where players write rap verses to battle each other." },
                            new Game { Name = "Patently Stupid", Description = "A game where players invent and pitch ridiculous inventions to solve problems." },
                            new Game { Name = "Zeeple Dome", Description = "A game where players fight aliens in an arena by launching themselves at them." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 6",
                        games = new Game[]
                        {
                            new Game { Name = "Trivia Murder Party 2", Description = "A sequel to Trivia Murder Party with new questions and features." },
                            new Game { Name = "Dictionarium", Description = "A game where players invent definitions for made-up words and vote on their favorites." },
                            new Game { Name = "Push the Button", Description = "A game where players try to determine who among them are aliens." },
                            new Game { Name = "Joke Boat", Description = "A game where players write and perform their own stand-up comedy routines." },
                            new Game { Name = "Role Models", Description = "A game where players try to match each other up with the most fitting personas." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 7",
                        games = new Game[]
                        {
                            new Game { Name = "Quiplash 3", Description = "A sequel to Quiplash with new prompts and features." },
                            new Game { Name = "The Devils and the Details", Description = "A cooperative game where players play as a family of devils trying to accomplish tasks in the mortal world." },
                            new Game { Name = "Champ'd Up", Description = "A drawing game where players create their own fighters to compete in a championship tournament." },
                            new Game { Name = "Talking Points", Description = "A game where players give presentations using slides they haven't seen before." },
                            new Game { Name = "Blather 'Round", Description = "A game where players try to get others to guess a secret phrase without using any of the forbidden words." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 8",
                        games = new Game[]
                        {
                            new Game { Name = "Drawful Animate", Description = "Drawful but now with Animation !" },
                            new Game { Name = "The Wheel of Enourmous Proportions", Description = "Answer Trivia Question and get lucky to win the wheel !" },
                            new Game { Name = "Job Job", Description = "Answer Questions. Win a Job." },
                            new Game { Name = "The Poll Mine", Description = "Answer Questions, Pick Doors, maybe surive." },
                            new Game { Name = "Weapons Drawn", Description = "Hide a letter in a murder weapon you design, the others will try to catch you." }
                        }
                    },
                    new Pack
                    {
                        Name = "Jackbox Party Pack 9",
                        games = new Game[]
                        {
                            new Game { Name = "Fibbage 4", Description = "A sequel of the sequel to Fibbage with new questions and features." },
                            new Game { Name = "Roomerang", Description = "Jackbox but it's a reality tv show" },
                            new Game { Name = "Junktopia", Description = "Buy trash and make a funny backstory to sell it" },
                            new Game { Name = "Nonesensory", Description = "Rank things between weird scales like the scale between apple and monkey." },
                            new Game { Name = "Quixort", Description = "Order tings, be better than the other team and win. It's so easy." }
                        }
                    },
                };
        }
    }
}
