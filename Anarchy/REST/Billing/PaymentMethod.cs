using Newtonsoft.Json;

namespace Discord
{
    public class PaymentMethod
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("type")]
        public int Type { get; private set; }


        [JsonProperty("invalid")]
        public bool Invalid { get; private set; }


        [JsonProperty("brand")]
        public string Brand { get; private set; }


        [JsonProperty("last_4")]
        public int Last4 { get; private set; }


        [JsonProperty("expires_month")]
        public int ExpirationMonth { get; private set; }


        [JsonProperty("expires_year")]
        public int ExpirationYear { get; private set; }


        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; private set; }


        [JsonProperty("country")]
        public string County { get; private set; }


        [JsonProperty("default")]
        public bool Default { get; private set; }
    }
}
