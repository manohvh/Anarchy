using Newtonsoft.Json;

namespace Discord
{
    public class GuildInvite : Invite
    {
        public GuildInvite()
        {
            OnClientUpdated += (sender, e) =>
            {
                Guild.SetClient(Client);
                Channel.SetClient(Client);
            };
        }


        [JsonProperty("guild")]
        public PartialGuild Guild { get; private set; }


        [JsonProperty("channel")]
        public GuildChannel Channel { get; private set; }


        [JsonProperty("temporary")]
        public bool Temporary { get; private set; }


        [JsonProperty("uses")]
        public uint Uses { get; private set; }


        [JsonProperty("max_uses")]
        public uint MaxUses { get; private set; }


        [JsonProperty("approximate_presence_count")]
        public uint OnlineMembers { get; private set; }


        [JsonProperty("approximate_member_count")]
        public uint TotalMembers { get; private set; }


        public GuildInvite Join()
        {
            return Client.JoinGuild(Code);
        }


        public override string ToString()
        {
            return Code;
        }
    }
}
