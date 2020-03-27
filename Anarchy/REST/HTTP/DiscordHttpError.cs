using Newtonsoft.Json;

namespace Discord
{
    internal class DiscordHttpError
    {
        [JsonProperty("code")]
        public DiscordError Code { get; private set; }


        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}
