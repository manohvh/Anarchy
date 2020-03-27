using Newtonsoft.Json;

namespace Discord
{
    public class PurchaseOptions
    {
        [JsonProperty("expected_amount")]
        public int ExpectedAmount { get; set; }


        [JsonProperty("gift")]
        private bool _gift = true; // rn we only have support for gifts kek


        [JsonProperty("payment_source_id")]
        public ulong PaymentMethodId { get; set; }


        [JsonProperty("sku_subscription_plan_id")]
        public ulong SkuPlanId { get; set; }
    }
}
