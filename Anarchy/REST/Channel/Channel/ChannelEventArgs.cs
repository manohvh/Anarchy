namespace Discord
{
    public class ChannelEventArgs
    {
        public GuildChannel Channel { get; private set; }

        public ChannelEventArgs(GuildChannel channel)
        {
            Channel = channel;
        }


        public override string ToString()
        {
            return Channel.ToString();
        }
    }
}