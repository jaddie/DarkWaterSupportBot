﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squishy.Irc.Commands;

namespace DarkwaterSupportBot
{
    class UtilityCommands
    {
        public class LeaveAMessage : Command
        {
            public LeaveAMessage()
                : base("leavemsg", "lm", "note")
            {
                Usage = "leavemsg nicktoleavefor message to send.";
                Description = "Use to leave a message for someone on IRC for when they come back online";
            }
            public override void Process(CmdTrigger trigger)
            {
                var nick = trigger.Args.NextWord();
                var messagetosend = trigger.Args.Remainder.Normalize();
                if (string.IsNullOrEmpty(nick) || string.IsNullOrEmpty(messagetosend))
                {
                    trigger.Reply("Failed to parse input, please try again");
                }
                else
                {
                    using (var db = new MessagesContainer())
                    {
                        var msg = new Message
                                      {
                                          DateLeft = DateTime.Now.ToString(),
                                          FromIrcNick = trigger.User.Nick,
                                          IrcNick = nick.ToLower(),
                                          MessageText = messagetosend.ToLower()
                                      };
                        db.Messages.AddObject(msg);
                        db.SaveChanges();
                        trigger.Reply("Message saved");
                        return;
                    }
                }
            }
        }

    }
}
