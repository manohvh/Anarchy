using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceSpeaking
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("int")]
        public int SSRC { get; private set; }


        [JsonProperty("speaking")]
        public int State { get; private set; }
    }
}
