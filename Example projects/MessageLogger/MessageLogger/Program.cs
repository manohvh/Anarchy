using System;
using System.Threading;
using Discord;
using Discord.Gateway;

namespace MessageLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordSocketClient client = new DiscordSocketClient();
            client.OnLoggedIn += OnLoggedIn;
            client.OnMessageReceived += OnMessageReceived;

            Console.Write("Token: ");
            client.Login(Console.ReadLine());

            Thread.Sleep(-1);
        }


        private static void OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine($"Logged into {args.User}");
        }


        private static void OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            Console.WriteLine($"[{args.Message.ChannelId}/{args.Message.Author}] {args.Message.Content}");
        }
    }
}
