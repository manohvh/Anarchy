﻿using Newtonsoft.Json;

namespace Discord
{
    public class DiscoveryGuild : BaseGuild
    {
        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("approximate_presence_count")]
        public int OnlineMembers { get; private set; }


        [JsonProperty("approximate_member_count")]
        public int Members { get; private set; }


        [JsonProperty("premium_subscription_count")]
        public int PremiumSubscriptions { get; private set; }


        [JsonProperty("preferred_locale")]
        public string PreferredLocale { get; private set; }
    }
}
