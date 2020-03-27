using System;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordGiftCode
    {
        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("sku_id")]
        public ulong SkuId { get; private set; }


        [JsonProperty("application_id")]
        public ulong ApplicationId { get; private set; }


        [JsonProperty("expires_at")]
        private string _expiresAt;

        public DateTime ExpiresAt
        {
            get
            {
                return DiscordTimestamp.FromString(_expiresAt);
            }
        }


        [JsonProperty("redeemed")]
        public bool Redeemed { get; private set; }


        [JsonProperty("user")]
        public User Gifter { get; private set; }


        [JsonProperty("subscription_plan_id")]
        public ulong SubPlanId { get; private set; }
    }
}
