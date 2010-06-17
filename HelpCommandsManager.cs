using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkwaterSupportBot
{
    public class HelpCommandsManager
    {
        public static HelpCommandsEntities Help = new HelpCommandsEntities();
        public static HelpCommand Search(string cmdName)
        {
            return Enumerable.FirstOrDefault(Help.HelpCommands, helpCommand => helpCommand.HelpName == cmdName);
        }

        public static void Add(HelpCommand cmd)
        {
            Help.HelpCommands.AddObject(cmd);
            Help.SaveChanges();
        }
        public static void Delete(HelpCommand cmd)
        {
            Help.HelpCommands.DeleteObject(cmd);
            Help.SaveChanges();
        }
    }
}
