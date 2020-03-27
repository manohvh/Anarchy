using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class GatewayIdentification
    {
        [JsonProperty("token")]
        public string Token { get; set; }


        [JsonProperty("properties")]
        public SuperProperties Properties { get; set; }


        [JsonProperty("presence")]
        public Presence Presence { get; set; }


        [JsonProperty("compress")]
        public bool Compress { get; set; }


        public override string ToString()
        {
            return Token;
        }
    }
}
