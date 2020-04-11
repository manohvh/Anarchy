using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class GiftsExtensions
    {
        public static string PurchaseGift(this DiscordClient client, ulong paymentMethodId, ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return client.HttpClient.Post($"https://discordapp.com/api/v6/store/skus/{skuId}/purchase", new PurchaseOptions()
            {
                PaymentMethodId = paymentMethodId,
                SkuPlanId = subPlanId,
                ExpectedAmount = expectedAmount
            }).Deserialize<JObject>().Value<string>("gift_code");
        }


        public static IReadOnlyList<DiscordGift> GetGiftInventory(this DiscordClient client)
        {
            return client.HttpClient.Get("/users/@me/entitlements/gifts").Deserialize<IReadOnlyList<DiscordGift>>();
        }


        public static List<DiscordGiftCode> QueryGiftCodes(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return client.HttpClient.Get($"/users/@me/entitlements/gift-codes?sku_id={skuId}&subscription_plan_id={subPlanId}").Deserialize<List<DiscordGiftCode>>();
        }


        public static DiscordGiftCode CreateGiftCode(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return client.HttpClient.Post("/users/@me/entitlements/gift-codes", $"{{\"sku_id\":{skuId},\"subscription_plan_id\":{subPlanId}}}").Deserialize<DiscordGiftCode>();
        }
    }
}
