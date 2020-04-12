using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Discord;
using Discord.Gateway;

namespace MusicBot
{
    class Program
    {
        public static Dictionary<ulong, List<DiscordVoiceState>> VoiceStates { get; private set; }
        public static Dictionary<ulong, MusicSession> Sessions { get; private set; }
        public static readonly Color EmbedColor = Color.FromArgb(105, 125, 202);
        public static readonly EmbedFooter EmbedFooter = new EmbedFooter()
        {
            Text = "Powered by Anarchy",
            IconUrl = "https://cdn.discordapp.com/attachments/698872354599600220/698934060491210802/Anarchy.png"
        };

        static void Main(string[] args)
        {
            VoiceStates = new Dictionary<ulong, List<DiscordVoiceState>>();
            Sessions = new Dictionary<ulong, MusicSession>();

            Console.Write("Token: ");
            string token = Console.ReadLine();

            DiscordSocketClient client = new DiscordSocketClient();
            client.CreateCommandHandler("m;");
            client.OnLoggedIn += Client_OnLoggedIn;
            client.OnJoinedGuild += Client_OnJoinedGuild;
            client.OnVoiceStateUpdated += Client_OnVoiceStateUpdated;
            client.Login(token);

            Thread.Sleep(-1);
        }

        private static void Client_OnVoiceStateUpdated(DiscordSocketClient client, VoiceStateEventArgs args)
        {
            if (args.State.UserId == client.User.Id)
                return;

            var voiceStates = VoiceStates[args.State.Guild.Id];

            for (int i = 0; i < voiceStates.Count; i++)
            {
                if (voiceStates[i].UserId == args.State.Member.User.Id)
                {
                    voiceStates[i] = args.State;

                    return;
                }
            }

            voiceStates.Add(args.State);
        }

        private static void Client_OnJoinedGuild(DiscordSocketClient client, SocketGuildEventArgs args)
        {
            VoiceStates.Add(args.Guild.Id, args.Guild.VoiceStates.ToList());
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Logged in");

            client.SetActivity(new StreamActivity() { Name = "powered by Anarchy", Url = "https://www.twitch.tv/ilinked" });

            if (client.User.Type == UserType.User)
            {
                foreach (var guild in args.Guilds)
                    VoiceStates.Add(guild.Id, guild.ToSocketGuild().VoiceStates.ToList());
            }
        }
    }
}
