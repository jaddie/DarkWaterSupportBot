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
            try
            {
                Help.HelpCommands.AddObject(cmd);
                Help.SaveChanges();
            }
            catch (Exception e)
            {
                UtilityMethods.Print("Error occured in the adding method - " + e.InnerException.Data + e.InnerException + e.InnerException.Source + e.InnerException.Message + e.InnerException.StackTrace,true);
            }
        }
        public static void Delete(HelpCommand cmd)
        {
            try
            {
                Help.HelpCommands.DeleteObject(cmd);
                Help.SaveChanges();
            }
            catch (Exception e)
            {
                UtilityMethods.Print("Error occured in the deleting method - " + e.InnerException.Data + e.InnerException + e.InnerException.Source + e.InnerException.Message + e.InnerException.StackTrace,true);
            }
        }
    }
}
