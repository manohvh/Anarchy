namespace Discord.Commands
{
    public abstract class Command
    {
        public string[] Arguments { get; private set; }
        public Message Message { get; private set; }

        public Command(string[] args, Message message)
        {
            Arguments = args;
            Message = message;
        }


        public abstract void Execute();
    }
}
