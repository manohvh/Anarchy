using Newtonsoft.Json;

namespace Discord
{
    public class NitroBoost
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("ended")]
        public bool Ended { get; private set; }


        public static implicit operator ulong(NitroBoost instance)
        {
            return instance.Id;
        }
    }
}
