using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceResponse
    {
        [JsonProperty("op")]
        public DiscordVoiceOpcode Opcode { get; private set; }


        [JsonProperty("d")]
        public dynamic Data { get; private set; }
    }
}
