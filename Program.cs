#region Used Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Squishy.Irc;
using Squishy.Irc.Account;
using Squishy.Irc.Commands;
using Squishy.Irc.Protocol;
using Squishy.Network;

#endregion

namespace DarkwaterSupportBot
{
    public class DarkWaterBot : IrcClient
    {
        #region MainExecution

        #region Fields

        #region Lists

        public static readonly List<string> ChannelList = new List<string> {"#woc","#dogfighter","#dogfighterbeta"};

        #endregion

        #region IRC Connection info
        public static int SendQueue
        {
            get { return ThrottledSendQueue.CharsPerSecond; }
            set { ThrottledSendQueue.CharsPerSecond = value; }
        }
        private const int Port = 6667;
        public static readonly DarkWaterBot Irc = new DarkWaterBot
                                                 {
                                                     Nicks = new[] {"DFBot","DFBot_","DFHelper"},
                                                     UserName = "DFBot",
                                                     Info = "Dogfighter Helper",
                                                     _network = Dns.GetHostAddresses("irc.quakenet.org")
                                                 };
        public static System.Timers.Timer SpamTimer = new System.Timers.Timer();
        private IPAddress[] _network;
        #endregion

        #region Other

        public static string LogFile;
        public static string ReplyChan = "#woc";
        public static readonly Stopwatch Runtimer = new Stopwatch();
        public static Process Utility = Process.GetCurrentProcess();
        #endregion

        #endregion Fields

        public static void Main()
        {
            Irc.ProtocolHandler.PacketReceived += OnReceive;
            try
            {
                SpamTimer.Interval = 5000;
                SpamTimer.Elapsed += SpamTimer_Elapsed;
                Console.ForegroundColor = ConsoleColor.Green;
                #region IRC Connecting

                SendQueue = 80;
                Irc.Client.Connecting += OnConnecting;
                Irc.Client.Connected += Client_Connected;
                Irc.BeginConnect(Irc._network[0].ToString(), Port);
                #endregion
                Runtimer.Start();
                while (true) // Prevent WCell.Tools from crashing - due to console methods inside the program.
                {
                    var line = new StringStream(Console.ReadLine());
                    UtilityMethods.OnConsoleText(line);
                }
            }
                #region Main Exception Handling

            catch (Exception e)
            {
                UtilityMethods.Print(string.Format("Exception {0} \n {1}", e.Message, e.StackTrace), true);
                WriteErrorSystem.WriteError(new List<string> {"Exception:", e.Message, e.StackTrace});
                foreach (var chan in ChannelList)
                {
                    Irc.CommandHandler.Msg(chan, "Main Execution error, shutting down!");
                }
                Console.WriteLine("Closing in 5 seconds");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

            #endregion
        }

        static void SpamTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SpamTimer.Stop();
        }
        protected static void OnReceive(IrcPacket packet)
        {
                Console.WriteLine("<-- " + packet);
        }

        protected override void OnBeforeSend(string text)
        {
                Console.WriteLine("--> " + text);
        }
        public static void Client_Connected(Connection con)
        {
            UtilityMethods.Print("Connected to IRC Server", true);
        }
        public static void Irc_Disconnected(IrcClient arg1, bool arg2)
        {
            try
            {
                UtilityMethods.Print("Disconnected from IRC server, Attempting reconnect in 5 seconds", true);
                Thread.Sleep(5000);
                Irc.BeginConnect(Irc._network[0].ToString(), Port);
            }
            catch (Exception e)
            {
                UtilityMethods.Print(e.Data + e.StackTrace,true);
            }

        }

        #region IrcSystem
        public static void OnConnecting(Connection con)
        {
            UtilityMethods.Print("Connecting to IRC server", true);
            UtilityMethods.Print(DateTime.Now + " : Connecting to server");
        }
        protected override void Perform()
        {
            try
            {
                IrcCommandHandler.Initialize(); // This is what parses all coded commands
                CommandHandler.RemoteCommandPrefix = "!";
                Irc.CommandHandler.Mode(Me.Nick + " +x");
                Irc.CommandHandler.Msg("Q@CServe.quakenet.org","auth DFBot mehlolkek1");
                foreach (var chan in ChannelList)
                {
                    if (chan.Contains(","))
                    {
                        var chaninfo = chan.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (chaninfo.Length > 1)
                            CommandHandler.Join(chaninfo[0], chaninfo[1]);
                        else
                            CommandHandler.Join(chaninfo[0]);
                    }
                    else
                    {
                        CommandHandler.Join(chan);
                    }
                }
            }
            catch (Exception e)
            {
                UtilityMethods.Print(e.Message + e.Data + e.StackTrace, true);
            }
        }
        protected override void OnUnknownCommandUsed(CmdTrigger trigger)
        {
            if (trigger.Alias == null)
            {
                return;
            }
            HelpCommand cmd = HelpCommandsManager.Search(trigger.Alias.ToLower());
            if(trigger.User.AccountLevel == AccountMgr.AccountLevel.User || trigger.User.AccountLevel == AccountMgr.AccountLevel.Admin)
            trigger.Reply(cmd != null
                              ? cmd.HelpText
                              : "This command does not exist, try !help or suggest the creation of the command");
            else
                trigger.User.Msg(cmd != null
                              ? cmd.HelpText
                              : "This command does not exist, try !help or suggest the creation of the command");
        }

        protected override void OnQueryMsg(IrcUser user, StringStream text)
        {
            UtilityMethods.Print(user + text.String, true);
        }
        protected override void OnText(IrcUser user, IrcChannel chan, StringStream text)
        {
            try
            {
                if (text.String.ToLower().Contains("badger"))
                {
                    Random rand = new Random();
                    var randomint = rand.Next(0, 100);
                    if(randomint > 50)
                    {
                        chan.Msg("Ahhhh Snakkeeee its a snaakee!!!! http://www.youtube.com/watch?v=EIyixC9NsLI");
                    }
                    else
                    {
                        chan.Msg("Mushroom Mushroom Mushroom! http://www.youtube.com/watch?v=EIyixC9NsLI");
                    }
                }
                else
                {
                    if (text.String.ToLower().Contains("snake"))
                    {
                        chan.Msg("Badger...Badger...Badger...Badger.. http://www.youtube.com/watch?v=EIyixC9NsLI");
                    }
                    else
                    {
                        if (text.String.ToLower().Contains("mushroom"))
                        {
                            chan.Msg("Badger..Badger..Badger! http://www.youtube.com/watch?v=EIyixC9NsLI");
                        }
                    }
                }

                if (text.String.Contains("ACTION") && text.String.ToLower().Contains("help") && text.String.ToLower().Contains("bot"))
                {
                    if (chan != null)
                        Irc.CommandHandler.Describe(chan, FunCommands.ReactToAction(), chan.Args);
                    else
                        Irc.CommandHandler.Describe(user, FunCommands.ReactToAction(), user.Args);
                }

                #region MessagesSent

                UtilityMethods.Print(string.Format("User {0} on channel {1} Sent {2}", user, chan, text), true);

                #endregion
            }
            catch (Exception e)
            {
                CommandHandler.Msg("#woc", e.Message);
                UtilityMethods.Print(e.StackTrace + e.Message, true);
            }
        }
        public override bool MayTriggerCommand(CmdTrigger trigger, Command cmd)
        {
            try
            {
                if (base.MayTriggerCommand(trigger, cmd))
                {
                    if (trigger.Channel != null && trigger.Target == trigger.Channel && !trigger.Args.String.ToLower().StartsWith(CommandHandler.RemoteCommandPrefix + "help"))
                    {
                        if (!SpamTimer.Enabled)
                        {
                            SpamTimer.Start();
                            return true;
                        }
                        else
                        {
                            trigger.User.Msg("Don't try to make me spam!");
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + e.ToString());
                return false;
            }

            /*
            try
            {
                if (File.Exists("auth.txt") && cmd.Name.ToLower() != "addauth")
                {
                    using (var reader = new StreamReader("auth.txt"))
                    {
                        var usernames = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            usernames.Add(reader.ReadLine());
                        }
                        if (usernames.Any(username => username == trigger.User.AuthName))
                        {
                            return true;
                        }
                    }
                }
                return trigger.User.IsOn("#wcell.dev") || trigger.User.IsOn("#woc") || trigger.User.IsOn("#wcell");*/
        //TODO: Update to add a proper suited auth system.);
            //return true;
        }
        #endregion

        #endregion
    }
}