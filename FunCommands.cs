using System;
using Squishy.Irc.Commands;

namespace DarkwaterSupportBot

{
    class FunCommands
    {
        #region EightBallCommand

        public class EightBallCommand : Command
        {
            public EightBallCommand()
                : base("eightball", "eight", "eb")
            {
                Usage = "eightball DecisionQuestion";
                Description = "Provide an answer to decision";
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    string[] eightballanswers = {
                                                    "As I see it, yes",
                                                    "Ask again later",
                                                    "Better not tell you now",
                                                    "Cannot predict now",
                                                    "Concentrate and ask again",
                                                    "Don't count on it",
                                                    "It is certain",
                                                    "It is decidedly so",
                                                    "Most likely",
                                                    "My reply is no",
                                                    "My sources say no",
                                                    "Outlook good",
                                                    "Outlook not so good",
                                                    "Reply hazy, try again",
                                                    "Signs point to yes",
                                                    "Very doubtful",
                                                    "Without a doubt",
                                                    "Yes",
                                                    "Yes - definitely",
                                                    "You may rely on it"
                                                };
                    if (!String.IsNullOrEmpty(trigger.Args.Remainder))
                    {
                        var rand = new Random();
                        var randomchoice = rand.Next(0, 19);
                        trigger.Reply(eightballanswers[randomchoice]);
                    }
                    else
                    {
                        trigger.Reply("You didnt give me a decision question!");
                    }
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Data + e.StackTrace, true);
                }
            }
        }

        #endregion
        #region ActionCommand

        public class ActionCommand : Command
        {
            public ActionCommand()
                : base("Action", "Me")
            {
                Usage = "action -target destination action to write";
                Description = "Writes the provided Action.";
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    string target = trigger.Target.ToString();
                    if (trigger.Args.NextModifiers() == "target")
                    {
                        target = trigger.Args.NextWord();
                    }
                    DarkWaterBot.Irc.CommandHandler.Describe(target, trigger.Args.Remainder, trigger.Args);
                }
                catch (Exception e)
                {
                    trigger.Reply("I cant write that action, perhaps invalid target?");
                    trigger.Reply(e.Message);
                    UtilityMethods.Print(e.Data + e.StackTrace, true);
                }
            }
        }

        #endregion
        #region ReactToAction
        public static string ReactToAction()
        {
            try
            {
                string[] actions = {
                                       "dodges",
                                       "ducks",
                                       "evades",
                                       "parries",
                                       "blocks",
                                       "does the monkey dance"
                                   };
                var rand = new Random();
                var randomchoice = rand.Next(0, 5);
                return actions[randomchoice];
            }
            catch(Exception e)
            {
                UtilityMethods.Print(e.Data + e.StackTrace,true);
                return "";
            }
        }
        #endregion
    }
}
