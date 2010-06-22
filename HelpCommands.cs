using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squishy.Irc.Account;
using Squishy.Irc.Commands;

namespace DarkwaterSupportBot
{
    class HelpCommands
    {
        #region IrcAdditionCommands
        public class AddHelpCommand : Command
        {
            public AddHelpCommand()
                : base("addhelp")
            {
                Usage = "addhelp cmdtotrigger the text you want in the help reply text when the command is used";
                Description = "Add a new command to the help database";
                RequiredAccountLevel = AccountMgr.AccountLevel.Admin;
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var helpName = trigger.Args.NextWord().ToLower();
                    var helpText = trigger.Args.Remainder;
                    if(!string.IsNullOrEmpty(helpName) && !string.IsNullOrEmpty(helpText))
                    {
                        HelpCommandsManager.Add(HelpCommand.CreateHelpCommand(helpName,helpText));
                        trigger.Reply(string.Format("Added new command HelpTrigger: {0} HelpText: {1}",helpName,helpText));
                    }
                    else
                    {
                        trigger.User.Msg("An error occured adding the specified command, one of the values came out as null, please check and try again or report the error!");
                    }
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Message + e.Data + e.StackTrace, true);
                }
            }
        }
        public class DeleteHelpCommand : Command
        {
            public DeleteHelpCommand()
                : base("deletehelp")
            {
                Usage = "deletehelp cmdtotrigger";
                Description = "Remove a help command from the help database";
                RequiredAccountLevel = AccountMgr.AccountLevel.Admin;
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var helpName = trigger.Args.NextWord().ToLower();
                    if (!string.IsNullOrEmpty(helpName))
                    {
                        var cmd = HelpCommandsManager.Search(helpName);
                        if (cmd != null)
                        {
                            HelpCommandsManager.Delete(cmd);
                            trigger.Reply(string.Format("Deleted command HelpTrigger: {0} HelpText: {1}", cmd.HelpName, cmd.HelpText));
                        }
                        else
                        {
                            trigger.Reply("No command was found with that name!");
                        }
                    }
                    else
                    {
                        trigger.Reply("An error occured deleting the specified command, it appears that the command did not exist, please check and try again or report the error!");
                    }
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Message + e.Data + e.StackTrace, true);
                }
            }
        }
        #endregion
    }
}
