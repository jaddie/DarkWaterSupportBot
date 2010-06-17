using System;
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
            if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(password) || trigger == null)
            {
                throw new NullReferenceException();
            }
            if (!File.Exists("login.txt"))
            {
                throw new FileNotFoundException();
            }
            using (var reader = new StreamReader("login.txt"))
            {
                while(!reader.EndOfStream)
                {
                    if(reader.ReadLine().Contains(acc) && reader.ReadLine().Contains(password))
                    {
                        trigger.Reply("That account already exists!");
                        return;
                    }
                }
            }
            using (var write = new StreamWriter("login.txt", true))
            {
                write.WriteLine(acc + ":" + password + ":guest");
            }
            trigger.Reply("Account created");
        }

        public static void Login(CmdTrigger trigger, string acc, string password)
        {
            if (!File.Exists("login.txt"))
            {
                trigger.Reply("The login file was not found! Please use the create account command!");
                return;
            }

            if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(password))
            {
                trigger.Reply("Invalid login string");
                return;
            }
            using (StreamReader read = File.OpenText("login.txt"))
            {
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
                            trigger.Reply("Succesfully logged in");
                        }
                        else
                        {
                            trigger.Reply("Invalid password or account");
                        }
                    }
                    line = read.ReadLine();
                }
            }
        }
    }
}