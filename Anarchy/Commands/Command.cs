using Discord.Gateway;

namespace Discord.Commands
{
    public abstract class Command
    {
        public abstract void Execute(DiscordSocketClient client, string[] args, Message message);
    }
}
