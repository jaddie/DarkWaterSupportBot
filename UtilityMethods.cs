using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Squishy.Irc;
using Squishy.Irc.Account;
using Squishy.Irc.Commands;
using Squishy.Irc.Protocol;
using Squishy.Network;
namespace DarkwaterSupportBot
{
    public class UtilityMethods
    {
        #region ClearQueueCommand

        public class ClearQueueCommand : Command
        {
            public ClearQueueCommand()
                : base("ClearQueue", "CQ")
            {
                Usage = "ClearSendQueue";
                Description = "Command to clear the send queue. Useful if you want the bot to stop spamming";
            }

            public override void Process(CmdTrigger trigger)
            {
                var lines = trigger.Irc.Client.SendQueue.Length;
                trigger.Irc.Client.SendQueue.Clear();
                trigger.Reply("Cleared SendQueue of {0} lines", lines);
            }
        }

        #endregion
        #region Print
        public static void Print(string text, bool irclog = false)
        {
            try
            {
                Console.WriteLine(DateTime.Now + text);
                if (irclog)
                    DarkWaterBot.IrcLog.WriteLine(DateTime.Now + text);
            }
            catch(Exception e)
            {
                Console.WriteLine("Write Failure" + e.Data + e.StackTrace);
            }

        }
        #endregion
        #region ConsoleText
        public static void OnConsoleText(StringStream cText)
        {
            try
            {
                switch (cText.NextWord().ToLower())
                {
                    case "join":
                        {
                            if (cText.Remainder.Contains(","))
                            {
                                var chaninfo = cText.Remainder.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                                if (chaninfo.Length > 1)
                                    DarkWaterBot.Irc.CommandHandler.Join(chaninfo[0], chaninfo[1]);
                                else
                                    DarkWaterBot.Irc.CommandHandler.Join(chaninfo[0]);
                            }
                            else
                            {
                                DarkWaterBot.Irc.CommandHandler.Join(cText.Remainder);
                            }
                        }
                        break;
                    case "say":
                        {
                            var chan = cText.NextWord();
                            var msg = cText.Remainder;
                            DarkWaterBot.Irc.CommandHandler.Msg(chan, msg);
                        }
                        break;
                    case "quit":
                        {
                            Print("Shutting down due to console quit command..", true);
                            foreach (var chan in DarkWaterBot.ChannelList)
                            {
                                DarkWaterBot.Irc.CommandHandler.Msg(chan, "Shutting down in 5 seconds due to console quit command..");
                            }
                            Thread.Sleep(5000);
                            DarkWaterBot.Irc.Client.DisconnectNow();
                            Environment.Exit(0);
                        }
                        break;
                }
            }
            catch(Exception e)
            {
                Print(e.Data + e.StackTrace, true);
            }
        }
        #endregion
        #region HelpListCommands
        /// <summary>
        /// TODO: Use localized strings
        /// The help command is special since it generates output.
        /// This output needs to be shown in the GUI if used from commandline and 
        /// sent to the requester if executed remotely.
        /// </summary>
        public class HelpCommand : Command
        {
            public static int MaxUncompressedCommands = 3;

            public HelpCommand()
                : base("Help", "?")
            {
                Usage = "Help|? [<match>]";
                Description = "Shows an overview over all Commands or -if you specify a <match>- the help for any matching commands.";
            }

            public override void Process(CmdTrigger trigger)
            {
                foreach (var helpCommand in HelpCommandsManager.Help.HelpCommands)
                {
                    trigger.Reply(helpCommand.HelpName);
                }
                var match = trigger.Args.NextWord();
                IList<Command> cmds;
                if (match.Length > 0)
                {
                    cmds = new List<Command>();
                    foreach (var cmd in IrcCommandHandler.List)
                    {
                        if (cmd.Enabled &&
                            trigger.MayTrigger(cmd) &&
                            cmd.Aliases.FirstOrDefault(ali => ali.IndexOf(match, StringComparison.InvariantCultureIgnoreCase) != -1) != null)
                        {
                            cmds.Add(cmd);
                        }
                    }
                    if (cmds.Count == 0)
                    {
                        trigger.Reply("Could not find a command matching '{0}'", match);
                    }
                    else
                    {
                        trigger.Reply("Found {0} matching commands: ", cmds.Count);
                    }
                }
                else
                {
                    trigger.Reply("Use \"Help <searchterm>\" to receive help on a certain command. - All current commands you can access:");
                    cmds = IrcCommandHandler.List.Where(cmd => cmd.Enabled && trigger.MayTrigger(cmd)).ToList();
                }

                var line = "";
                foreach (var cmd in cmds)
                {
                    if (cmds.Count <= MaxUncompressedCommands)
                    {
                        var desc = string.Format("{0} ({1})", cmd.Usage, cmd.Description);
                        trigger.Reply(desc);
                    }
                    else
                    {
                        var info = cmd.Name;
                        string aliases = "";
                        int runs = 0;
                        foreach(var alias in cmd.Aliases)
                        {
                            runs = runs + 1;
                            aliases = aliases + alias;
                            if (runs < cmd.Aliases.Length)
                            {
                                aliases = aliases + ",";
                                continue;
                            }

                        }
                        info += " (" + aliases + ")  ";

                        if (line.Length + info.Length >= IrcProtocol.MaxLineLength)
                        {
                            trigger.Reply(line);
                            line = "";
                        }

                        line += info;
                    }
                }

                if (line.Length > 0)
                {
                    trigger.Reply(line);
                }
            }
        }
        #endregion
        #region NewAuthSystem
        public class Login : Command
        {
            public Login()
                : base("Login")
            {
                Description = "login to your account";
                Usage = "login accname pw";
            }

            public override void Process(CmdTrigger trigger)
            {
                char[] seperator = { ' ' };
                string[] accountLine = trigger.Args.Remainder.Split(seperator, 2);
                if (accountLine.Length >= 2)
                {
                    AccountMgr.Login(trigger, accountLine[0], accountLine[1]);
                    trigger.Reply(string.Format("You should now be logged in as {0}", accountLine[0]));
                }
                else
                {
                    trigger.Reply("Error invalid input 2 values were not found, please try again!");
                }
            }
        }
        public class CreateAccount : Command
        {
            public CreateAccount()
                : base("createaccount", "ca")
            {
                Description = "create a account";
                Usage = "createaccount accname pw";
            }

            public override void Process(CmdTrigger trigger)
            {
                char[] seperator = { ' ' };
                string[] accountLine = trigger.Args.Remainder.Split(seperator, 2);
                if (accountLine.Length < 2)
                {
                    trigger.Reply("Error invalid input 2 values were not found, please try again!");
                }
                else
                {
                    AccountMgr.CreateAccount(trigger, accountLine[0], accountLine[1]);
                }
            }
        }
        public class ChangeUserLevel : Command
        {
            public ChangeUserLevel()
                : base("ChangeUserLevel")
            {
                Description = "changes the user level of a user";
                Usage = "ChangeUserLevel nick userlevel";
                RequiredAccountLevel = AccountMgr.AccountLevel.Admin;
            }

            public override void Process(CmdTrigger trigger)
            {
                char[] seperator = { ' ' };
                string[] accountLine = trigger.Args.Remainder.Split(seperator, 2);
                if (accountLine.Length == 2)
                {
                    IrcUser user = trigger.irc.GetUser(accountLine[0]);
                    if (user != null)
                    {
                        if (accountLine[1].ToLower() == "guest")
                        {
                            user.SetAccountLevel(AccountMgr.AccountLevel.Guest);
                            trigger.Reply(string.Format("Account {0} set to level Guest",accountLine[0]));
                        }
                        if (accountLine[1].ToLower() == "user")
                        {
                            user.SetAccountLevel(AccountMgr.AccountLevel.User);
                            trigger.Reply(string.Format("Account {0} set to level User",accountLine[0]));
                        }
                        if (accountLine[1].ToLower() == "admin")
                        {
                            user.SetAccountLevel(AccountMgr.AccountLevel.Admin);
                            trigger.Reply(string.Format("Account {0} set to level Admin", accountLine[0]));
                        }
                    }
                    else
                    {
                        trigger.Reply("That nick appears to be invalid, please try again!");
                    }
                }
                else
                {
                    trigger.Reply("Invalid input was provided, please check and try again!");
                }
            }
        }
        #endregion
    }
}
