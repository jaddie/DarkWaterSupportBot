using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squishy.Irc.Commands;

namespace DarkwaterSupportBot
{
    class HelpCommands
    {
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

    }
}
