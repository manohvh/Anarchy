using Discord;
using Discord.Commands;
using Discord.Gateway;

namespace MusicBot
{
    [Command("loop", "Enables or disables the queue looping")]
    public class LoopCommand : Command
    {
        public override void Execute(DiscordSocketClient client, string[] args, Message message)
        {
            if (!Program.Sessions.ContainsKey(message.Guild))
                message.Channel.SendMessage("Bot is not connected to a voice channel.");
            else
            {
                Program.Sessions[message.Guild].Loop = !Program.Sessions[message.Guild].Loop;

                message.Channel.SendMessage("Looping has been set to: " + Program.Sessions[message.Guild].Loop);
            }
        }
    }
}
