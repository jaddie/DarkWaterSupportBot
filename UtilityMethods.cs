using System;
using System.Text;
using System.Threading;
using Squishy.Network;

namespace DarkwaterSupportBot
{
    public class UtilityMethods
    {
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
        public static void AddCommandsFromDatabase()
        {
            
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
    }
}
