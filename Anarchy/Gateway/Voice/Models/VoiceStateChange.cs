using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class VoiceStateChange
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("channel_id")]
        public ulong? ChannelId { get; set; }


        [JsonProperty("self_mute")]
        public bool Muted { get; set; }


        [JsonProperty("self_deaf")]
        public bool Deafened { get; set; }
    }
}
