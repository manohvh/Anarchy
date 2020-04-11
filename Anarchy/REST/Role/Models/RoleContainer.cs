using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class RoleContainer : Controllable
    {
        public RoleContainer()
        {
            OnClientUpdated += (sender, e) => Role.SetClient(Client);
        }

        private ulong _guildId;
        [JsonProperty("guild_id")]
        public ulong GuildId
        {
            get { return _guildId; }
            set { Role.GuildId = _guildId = value; }
        }


        [JsonProperty("role")]
        public DiscordRole Role { get; private set; }


        public override string ToString()
        {
            return Role.ToString();
        }
    }
}
