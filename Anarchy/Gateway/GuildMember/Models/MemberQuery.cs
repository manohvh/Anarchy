using Newtonsoft.Json;

namespace Discord.Gateway
{
    /// <summary>
    /// Query for getting a list of guild members
    /// </summary>
    internal class GatewayMemberQuery
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("query")]
#pragma warning disable CS0414, IDE0044, IDE0051
        private string _query = "";
#pragma warning restore CS0414, IDE0044, IDE0051


        [JsonProperty("limit")]
        public uint Limit { get; set; }


        public override string ToString()
        {
            return GuildId.ToString();
        }
    }
}
