using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public class VersionCommand : Command
        {
            public VersionCommand()
                : base("Version")
            {
                Usage = "Version";
                Description = "Shows the version of this bot.";
            }

            public override void Process(CmdTrigger trigger)
            {
                //trigger.Reply(IrcClient.Version);
                AssemblyName asmName = Assembly.GetAssembly(GetType()).GetName();
                trigger.Reply(asmName.Name + ", v" + asmName.Version);
            }
        }
        public class ClearQueueCommand : Command
        {
            public ClearQueueCommand()
                : base("ClearQueue", "CQ")
            {
                Usage = "ClearSendQueue";
                Description = "Command to clear the send queue. Useful if you want the bot to stop spamming";
                RequiredAccountLevel = AccountMgr.AccountLevel.User;
            }

            public override void Process(CmdTrigger trigger)
            {
                var lines = trigger.Irc.Client.SendQueue.Length;
                trigger.Irc.Client.SendQueue.Clear();
                trigger.Reply("Cleared SendQueue of {0} lines", lines);
            }
        }
        public static void Print(string text, bool irclog = false)
        {
            try
            {
                Console.WriteLine(DateTime.Now + text);
                if (irclog)
                    using(var ircLog = new StreamWriter("Irclog.log",true))
                    {
                        ircLog.WriteLine(DateTime.Now + text);
                    }
            }
            catch(Exception e)
            {
                Console.WriteLine("Write Failure" + e.Data + e.StackTrace);
            }

        }
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
        public class SetSendQueue : Command
        {
            public SetSendQueue()
                : base("setsendqueue")
            {
                Usage = "setsendqueue number";
                Description = "Sets the throttle to number";
                RequiredAccountLevel = AccountMgr.AccountLevel.Admin;
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var sendqueue = trigger.Args.NextInt(60);
                    DarkWaterBot.SendQueue = sendqueue;
                    trigger.Reply("Set sendqueue to " + sendqueue);
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Message + e.Data + e.StackTrace, true);
                }
            }
        }
        #region SetDisplayIrcPacketsCommand

        public class SetDisplayIrcPacketsCommand : Command
        {
            public SetDisplayIrcPacketsCommand() : base("displaypackets")
            {
                Usage = "displaypackets true";
                Description = "Sets displaying the irc packets on or off in the console.";
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var setting = Convert.ToBoolean(trigger.Args.NextWord());
                    if (!string.IsNullOrEmpty(Convert.ToString(setting)))
                    {
                        DarkWaterBot.DisplayIrcPackets = setting;
                    }
                    else
                    {
                        trigger.Reply("Error parsing please check input, you may use true or false");
                    }
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Message + e.Data + e.StackTrace + e.Source + e.InnerException, true);
                }
            }
        }

        #endregion
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
                        trigger.User.Msg("Could not find a command matching '{0}'", match);
                    }
                    else
                    {
                        trigger.User.Msg("Found {0} matching commands: ", cmds.Count);
                    }
                }
                else
                {
                    string helpcmds = "";
                    int helpruns = 0;
                    foreach (var helpCommand in HelpCommandsManager.Help.HelpCommands)
                    {
                        if (string.IsNullOrEmpty(helpCommand.HelpName))
                        {
                            continue;
                        }
                        helpruns = helpruns + 1;
                        helpcmds = helpcmds + helpCommand.HelpName + ", ";
                    }
                    trigger.User.Msg("The following commands are help triggers, you cannot use the !help command with these!\n" + helpcmds);
                    trigger.User.Msg("Use \"Help <searchterm>\" to receive help on a certain command. - All current commands you can access:");
                    cmds = IrcCommandHandler.List.Where(cmd => cmd.Enabled && trigger.MayTrigger(cmd)).ToList();
                }

                var line = "";
                foreach (var cmd in cmds)
                {
                    if (cmds.Count <= MaxUncompressedCommands)
                    {
                        var desc = string.Format("{0} ({1})", cmd.Usage, cmd.Description);
                        trigger.User.Msg(desc);
                    }
                    else
                    {
                        var temp = line;
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
                            trigger.User.Msg(temp);
                            line = "";
                        }

                        line += info;
                    }
                }

                if (line.Length > 0)
                {
                    trigger.User.Msg(line);
                }
            }
        }
        #region NewAuthSystem
        public class LoginCommand : Command
        {
            public LoginCommand()
                : base("Login")
            {
                Description = "login to your account";
                Usage = "login accname pw";
            }

            public override void Process(CmdTrigger trigger)
            {
                var username = trigger.Args.NextWord();
                var password = trigger.Args.NextWord();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    trigger.Reply("Error invalid input please try again!");
                    return;
                }
                Login(trigger,username,password);
            }
            public static void Login(CmdTrigger trigger, string username, string password)
            {
                using (var accounts = new AccountsEntities())
                {
                    var authed = false;
                    foreach (var account in accounts.Accounts.Where(account => account.Username == username && account.Password == password))
                    {
                            switch (account.UserLevel)
                            {
                                case "guest":
                                    {
                                        authed = true;
                                        trigger.User.SetAccountLevel(AccountMgr.AccountLevel.Guest);
                                        trigger.Reply(string.Format("Logged in as {0} with level {1}", account.Username,account.UserLevel));
                                    }
                                break;
                                case "user":
                                    {
                                        authed = true;
                                        trigger.User.SetAccountLevel(AccountMgr.AccountLevel.User);
                                        trigger.Reply(string.Format("Logged in as {0} with level {1}", account.Username,account.UserLevel));
                                    }
                                break;
                                case "admin":
                                    {
                                        authed = true;
                                        trigger.User.SetAccountLevel(AccountMgr.AccountLevel.Admin);
                                        trigger.Reply(string.Format("Logged in as {0} with level {1}", account.Username,account.UserLevel));
                                    }
                                break;
                            }
                    }
                    if(!authed)
                    trigger.Reply("Account data invalid! Please try again!");
                }
            }
        }
        public class CreateAccountCommand : Command
        {
            public CreateAccountCommand()
                : base("createaccount", "ca")
            {
                Description = "Create a account";
                Usage = "createaccount accname pw";
            }

            public override void Process(CmdTrigger trigger)
            {
                var username = trigger.Args.NextWord();
                var password = trigger.Args.NextWord();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    trigger.Reply("Error invalid input please try again!");
                }
                else
                {
                    using (var accounts = new AccountsEntities())
                    {
                        if (Enumerable.Any(accounts.Accounts.Where(account => account.Username == username)))
                        {
                            trigger.Reply("That account already exists!");
                            return;
                        }
                    }
                    AddAccount(trigger, username,password);
                    trigger.Reply("Account created");
                }
            }
            public static void AddAccount(CmdTrigger trigger, string username, string password)
            {
                using (var accounts = new AccountsEntities())
                {
                    var account = new Account {Username = username, Password = password, UserLevel = "guest"};
                    accounts.Accounts.AddObject(account);
                    accounts.SaveChanges();
                }
            }
        }
        public static bool ValidateUserLevel(string userlevel)
        {
            if("admin user guest".Contains(userlevel.ToLower()))
            {
                return true;
            }
            return false;
        }
        public class DeleteAccountCommand : Command
        {
            public DeleteAccountCommand()
                : base("deleteaccount","da")
            {
                Usage = "deleteaccount username";
                Description = "Removes the account as specified";
                RequiredAccountLevel = AccountMgr.AccountLevel.Admin;
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var username = trigger.Args.NextWord();
                    if(string.IsNullOrEmpty(username))
                    {
                        trigger.Reply("Please specify username!");
                    }
                    else
                    {
                        using(var accounts = new AccountsEntities())
                        {
                            foreach (var account in accounts.Accounts.Where(account => account.Username == username))
                            {
                                accounts.DeleteObject(account);
                                accounts.SaveChanges();
                                trigger.Reply("Account deleted!");
                                return;
                            }
                            trigger.Reply("Account not found!");
                        }
                    }
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Message + e.Data + e.StackTrace, true);
                }
            }
        }
        public class ChangeUserLevelCommand : Command
        {
            public ChangeUserLevelCommand()
                : base("ChangeUserLevel")
            {
                Description = "Changes the user level of a user";
                Usage = "ChangeUserLevel nick userlevel";
                RequiredAccountLevel = AccountMgr.AccountLevel.Admin;
            }

            public override void Process(CmdTrigger trigger)
            {
                var username = trigger.Args.NextWord();
                var userlevel = trigger.Args.NextWord().ToLower();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userlevel))
                {
                    trigger.Reply("Invalid input please try again!");
                }
                else
                {
                    if (!ValidateUserLevel(userlevel))
                    {
                        trigger.Reply("Invalid userlevel specified, options are guest,user,admin");
                        return;
                    }
                    using (var accounts = new AccountsEntities())
                    {
                        foreach (var account in accounts.Accounts.Where(account => account.Username == username))
                        {
                            account.UserLevel = userlevel;
                            trigger.Reply("Account level changed to " + userlevel);
                            accounts.SaveChanges();
                            return;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
