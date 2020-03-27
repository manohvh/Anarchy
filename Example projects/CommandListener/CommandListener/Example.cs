using Discord;
using Discord.Commands;
using System;

namespace CommandListener
{
    [Command("example")]
    public class Example : Command
    {
        // This can be ignored. All it does is prepare the command context
        public Example(string[] args, Message message) : base(args, message)
        { }

        // This will be executed whenever the command ;example is sent through a channel
        public override void Execute()
        {
            Console.WriteLine("Author: " + Message.Author.ToString());
        }
    }
}
