using System;

namespace Discord
{
    public class Nitro
    {
        public NitroType Type { get; private set; }
        public DateTime? Since { get; private set; }


        public Nitro(string since)
        {
            if (since != null)
            {
                Type = NitroType.Unknown;
                Since = DiscordTimestamp.FromString(since);
            }
        }


        public override string ToString()
        {
            return Since.ToString();
        }
    }
}
