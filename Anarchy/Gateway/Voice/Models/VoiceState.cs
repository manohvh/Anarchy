using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class VoiceState : Controllable
    {
        public VoiceState()
        {
            OnClientUpdated += (sender, e) => Member.SetClient(Client);
        }


        [JsonProperty("channel_id")]
        public ulong? ChannelId { get; private set; }


        [JsonProperty("member")]
        private GuildMember _member;

        public GuildMember Member { get; private set; }


        private ulong? _guildId;
        [JsonProperty("guild_id")]
        public ulong? GuildId
        {
            get { return _guildId; }
            set
            {
                _guildId = value;

                if (Member != null)
                    Member.GuildId = _guildId.Value;
            }
        }


        [JsonProperty("mute")]
        public bool Muted { get; private set; }


        [JsonProperty("deaf")]
        public bool Deafened { get; private set; }


        [JsonProperty("suppress")]
        public bool Suppressed { get; private set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }
    }
}
