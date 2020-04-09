using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Discord
{
    public static class NitroExtensions
    {
        public static DiscordNitroGift GetNitroGift(this DiscordClient client, string code)
        {
            return client.HttpClient.Get($"/entitlements/gift-codes/{code}?with_application=false&with_subscription_plan=true")
                                .Deserialize<DiscordNitroGift>();
        }


        public static void RedeemNitroGift(this DiscordClient client, string code, ulong? channelId = null)
        {
            client.HttpClient.Post($"/entitlements/gift-codes/{code}/redeem", channelId.HasValue ? $"{{\"channel_id\":{channelId.Value}}}" : "");
        }


        public static string PurchaseNitroGift(this DiscordClient client, ulong paymentMethodId, DiscordNitroSubType type)
        {
            return client.PurchaseGift(paymentMethodId, type.SkuId, type.SubscriptionPlanId, type.ExpectedAmount);
        }


        public static List<NitroBoost> GetNitroBoosts(this DiscordClient client)
        {
            return client.HttpClient.Get("/users/@me/guilds/premium/subscription-slots").Deserialize<List<NitroBoost>>();
        }


        public static void BoostGuild(this DiscordClient client, ulong guildId)
        {
            client.HttpClient.Put($"/guilds/{guildId}/premium/subscriptions");
        }


        public static void RemoveGuildBoost(this DiscordClient client, ulong guildId, ulong subscriptionId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}/premium/subscriptions/{subscriptionId}");
        }


        public static DateTime GetBoostCooldown(this DiscordClient client, ulong subscriptionId)
        {
            return (DateTime)client.HttpClient.Get($"/users/@me/guilds/premium/subscriptions/{subscriptionId}/cooldown")
                                                            .Deserialize<JObject>().GetValue("ends_at").ToObject(typeof(DateTime));
        }
    }
}
