using Discord;
using Discord.Commands;
using Discord.Gateway;
using Discord.Voice;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot
{
    [Command("join", "Makes the bot join the voice channel you're currently in")]
    public class JoinCommand : Command
    {
        public override void Execute(DiscordSocketClient client, string[] args, Message message)
        {
            VoiceChannel channel;

            try
            {
                channel = client.GetChannel(Program.VoiceStates[message.Guild.Id].First(s => s.UserId == message.Author.User.Id).Channel.Id).ToVoiceChannel();
            }
            catch
            {
                message.Channel.SendMessage("You must be connected to a voice channel to play music");

                return;
            }

            if (!Program.Sessions.ContainsKey(message.Guild.Id) || Program.Sessions[message.Guild.Id].Channel.Id != channel.Id)
            {
                DiscordVoiceClient voiceClient = client.JoinVoiceChannel(message.Guild, channel, false, true);

                voiceClient.OnConnected += (c, e) =>
                {
                    var session = new MusicSession(message.Guild)
                    {
                        Client = voiceClient,
                        Channel = channel,
                        Queue = new Queue<Track>(),
                    };

                    Program.Sessions.Add(message.Guild.Id, session);

                    message.Channel.SendMessage("Connected to voice channel.");

                    Task.Run(() => session.StartQueue());
                };

                voiceClient.Connect();
            }
        }
    }
}
