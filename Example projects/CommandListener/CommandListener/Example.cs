using Discord;
using Discord.Commands;
using System;

namespace CommandListener
{
    [Command("example")]
    public class Example : Command
    {
        // This will be executed whenever the command ;example is sent through a channel
        public override void Execute(string[] args, Message message)
        {
            Console.WriteLine("Author: " + message.Author.ToString());

            message.Channel.SendMessage("alright alright, jeez");
        }
    }
}
