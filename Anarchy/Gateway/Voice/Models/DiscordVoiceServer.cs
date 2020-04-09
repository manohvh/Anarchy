using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordVoiceServer
    {
        [JsonProperty("token")]
        public string Token { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("endpoint")]
        public string Server { get; private set; }
    }
}