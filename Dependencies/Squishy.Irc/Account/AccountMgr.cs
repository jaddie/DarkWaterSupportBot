using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squishy.Irc.Commands;
using System.IO;

namespace Squishy.Irc.Account
{
    public class AccountMgr
    {
        public enum AccountLevel
        {
            Guest = 1,
            User = 2,
            Admin = 3
        }

        public static void CreateAccount(CmdTrigger trigger, string acc, string password)
        {
            if (acc == string.Empty || acc == null || password == string.Empty || password == null)
            {
                trigger.Reply("Invalid accname or password string");
                return;
            }

            StreamWriter write;
            if (!File.Exists("login.txt"))
            {
                write = File.CreateText("login.txt");
                write.WriteLine(acc + ":" + password + ":guest");
                write.Close();
                trigger.Reply("account created");
            }
            else
            {
                write = File.AppendText("login.txt");
                write.WriteLine(acc + ":" + password + ":guest");
                write.Close();
                trigger.Reply("account created");
            }
        }

        public static void Login(CmdTrigger trigger, string acc, string password)
        {
            if (!File.Exists("login.txt"))
            {
                trigger.Reply("login.txt wasnt found creating default login.txt, please edit the default login.txt and login again");
                CreateDefaultLogin();
                return;
            }

            if (acc == string.Empty || acc == null || password == string.Empty || password == null)
            {
                trigger.Reply("Invalid login string");
                return;
            }

            StreamReader read = File.OpenText("login.txt");
            
            string line = read.ReadLine();
            char[] seperator = { ':' };
            while (line != null)
            {
                string[] splittedLine = line.Split(seperator, 3);

                if (splittedLine.Length >= 3)
                {
                    if (splittedLine[0] == acc && splittedLine[1] == password)
                    {
                        if (splittedLine[2].ToLower() == "guest")
                        {
                            trigger.User.SetAccountLevel(AccountLevel.Guest);
                        }
                        if (splittedLine[2].ToLower() == "user")
                        {
                            trigger.User.SetAccountLevel(AccountLevel.User);
                        }
                        if (splittedLine[2].ToLower() == "admin")
                        {
                            trigger.User.SetAccountLevel(AccountLevel.Admin);
                        }
                        trigger.Reply("succesfully logged in");
                    }
                    else
                    {
                        trigger.Reply("invalid password or account");
                    }
                }
                line = read.ReadLine();
            }
            read.Close();

        }

        private static void CreateDefaultLogin()
        {
            StreamWriter write;
            if (!File.Exists("login.txt"))
            {
                write = File.CreateText("login.txt");
                string defaultAcc = "accname";
                string defaultPass = "password";
                write.WriteLine(defaultAcc + ":" + defaultPass + ":admin");
                write.Close();
            }
        }
    }
}