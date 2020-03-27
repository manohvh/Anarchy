using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Discord.Commands
{
    public class CommandHandler
    {
        private Dictionary<string, Type> _commands;
        public string Prefix { get; private set; }

        public CommandHandler(string prefix, DiscordSocketClient client)
        {
            Prefix = prefix;
            client.OnMessageReceived += Client_OnMessageReceived;
            _commands = new Dictionary<string, Type>();

            Assembly executable = Assembly.GetEntryAssembly();

            foreach (var type in executable.GetTypes())
            {
                dynamic[] attrs = (dynamic[])type.GetCustomAttributes().Cast<dynamic>();

                foreach (var attr in attrs)
                {
                    if (attr.TypeId.Name == "CommandAttribute")
                    {
                        _commands.Add(attr.Command, type);

                        break;
                    }
                }
            }
        }

        private void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            Task.Run(() =>
            {
                if (args.Message.Content.StartsWith(Prefix))
                {
                    string[] contents = args.Message.Content.Split(' ');

                    if (Prefix.Length < contents[0].Length)
                    {
                        string command = new string(contents[0].Skip(Prefix.Length).ToArray());

                        if (TryGetCommand(command, out KeyValuePair<string, Type> cmd))
                        {
                            object classInstance = Activator.CreateInstance(cmd.Value, contents.Skip(1).ToArray(), args.Message);

                            MethodInfo cmdMethod = cmd.Value.GetMethod("Execute");

                            cmdMethod.Invoke(classInstance, null);
                        }
                    }
                }
            });
        }

        private bool TryGetCommand(string command, out KeyValuePair<string, Type> commandHandler)
        {
            foreach (var cmd in _commands)
            {
                if (cmd.Key == command)
                {
                    commandHandler = cmd;

                    return true;
                }
            }

            commandHandler = new KeyValuePair<string, Type>();

            return false;
        }
    }
}
