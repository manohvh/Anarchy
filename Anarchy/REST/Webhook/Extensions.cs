using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Webhook
{
    public static class WebhookExtensions
    {
        #region management
        /// <summary>
        /// Creates a webhook
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for creating/modifying the webhook</param>
        /// <returns>The created webhook</returns>
        public static Hook CreateChannelWebhook(this DiscordClient client, ulong channelId, WebhookProperties properties)
        {
            properties.ChannelId = channelId;
            Hook hook = client.HttpClient.Post($"/channels/{channelId}/webhooks", 
                                    JsonConvert.SerializeObject(properties)).Deserialize<Hook>().SetClient(client);
            hook.Modify(properties);
            return hook;
        }


        /// <summary>
        /// Modifies a webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="properties">Options for modifyiing a webhook</param>
        /// <returns>The modified webhook</returns>
        public static Hook ModifyChannelWebhook(this DiscordClient client, ulong webhookId, WebhookProperties properties)
        {
            return client.HttpClient.Patch($"/webhooks/{webhookId}", 
                                JsonConvert.SerializeObject(properties)).Deserialize<Hook>().SetClient(client);
        }


        /// <summary>
        /// Deletes a webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        public static void DeleteChannelWebhook(this DiscordClient client, ulong webhookId, string token = null)
        {
            client.HttpClient.Delete($"/webhooks/{webhookId}/{token}");
        }
        #endregion


        /// <summary>
        /// Gets a webhook (if <paramref name="token"/> is set the client does not have to be authenticated)
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="token">The webhooks's token</param>
        public static Hook GetWebhook(this DiscordClient client, ulong webhookId, string token = "")
        {
            return client.HttpClient.Get($"/webhooks/{webhookId}/{token}")
                            .Deserialize<Hook>().SetClient(client);
        }


        /// <summary>
        /// Gets a guild's webhooks
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<Hook> GetGuildWebhooks(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Get($"/guilds/{guildId}/webhooks")
                                .Deserialize<IReadOnlyList<Hook>>().SetClientsInList(client);
        }


        /// <summary>
        /// Gets a channel's webhooks
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<Hook> GetChannelWebhooks(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}/webhooks")
                                .Deserialize<IReadOnlyList<Hook>>().SetClientsInList(client);
        }
    }
}
