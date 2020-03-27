using Newtonsoft.Json;

namespace Discord
{
    public class PartialGuild : BaseGuild
    {
        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }


        [JsonProperty("vanity_url_code")]
        public string VanityInvite { get; private set; }


        [JsonProperty("owner")]
        public bool Owner { get; private set; }


        [JsonProperty("permissions")]
#pragma warning disable IDE0051, IDE1006
        private uint _permissions
        {
            set { Permissions = new Permissions(value); }
        }
#pragma warning restore IDE0051, IDE1006
        public Permissions Permissions { get; private set; }


        /// <summary>
        /// Gets the full guild (<see cref="Guild"/>)
        /// </summary>
        public Guild GetGuild()
        {
            return Client.GetGuild(Id);
        }
    }
}
