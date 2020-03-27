using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Command { get; private set; }

        public CommandAttribute(string cmd)
        {
            Command = cmd;
        }
    }
}
