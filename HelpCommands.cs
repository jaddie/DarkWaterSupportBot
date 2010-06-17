using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squishy.Irc.Commands;

namespace DarkwaterSupportBot
{
    class HelpCommands
    {
        #region IrcAdditionCommands
        #region AddHelpCommand

        public class AddHelpCommand : Command
        {
            public AddHelpCommand()
                : base("addhelp")
            {
                Usage = "addhelp cmdtotrigger the text you want in the help reply text when the command is used";
                Description = "Add a new command to the help database";
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var helpName = trigger.Args.NextWord();
                    var helpText = trigger.Args.Remainder;
                    if(helpName != null && helpText != null)
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
                    UtilityMethods.Print(e.Data + e.StackTrace, true);
                }
            }
        }

        #endregion
        #region DeleteHelpCommand
        public class DeleteHelpCommand : Command
        {
            public DeleteHelpCommand()
                : base("deletehelp")
            {
                Usage = "deletehelp cmdtotrigger";
                Description = "Remove a help command from the help database";
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    var helpName = trigger.Args.NextWord();
                    if (helpName != null)
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
                        trigger.User.Msg("An error occured deleting the specified command, it appears that the command did not exist, please check and try again or report the error!");
                    }
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Data + e.StackTrace, true);
                }
            }
        }

        #endregion
        #endregion
        #region CodedHelpCommands
        #region PhysxCommand

        public class PhysxCommand : Command
        {
            public PhysxCommand()
                : base("Physx","ati")
            {
                Usage = "physx";
                Description = "Shows how to solve physx issues encountered on startup";
            }

            public override void Process(CmdTrigger trigger)
            {
                try
                {
                    trigger.Reply("1. Navigate to dogfighter install directory 2. Go to the Redist folder 3. Create a shortcut for PhysX_9.10.0129_SystemSoftware on the desktop 4. Edit the shortcut and put /passive on the end of the target line 5. Run the shortcut 6. Restart Steam when it is done 7. Launch Game!");
                }
                catch (Exception e)
                {
                    UtilityMethods.Print(e.Data + e.StackTrace, true);
                }
            }
        }

        #endregion
        #endregion
    }
}
