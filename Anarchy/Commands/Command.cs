namespace Discord.Commands
{
    public abstract class Command
    {
        public abstract void Execute(string[] args, Message message);
    }
}
