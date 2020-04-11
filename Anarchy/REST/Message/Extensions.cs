using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Discord
{
    public static class MessageExtensions
    {
        #region management
        /// <summary>
        /// Sends a message to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        /// <returns>The message</returns>
        public static Message SendMessage(this DiscordClient client, ulong channelId, string message, bool tts = false, Embed embed = null)
        {
            return client.HttpClient.Post($"/channels/{channelId}/messages", new MessageProperties() { Content = message, Tts = tts, Embed = embed })
                                .Deserialize<Message>().SetClient(client);
        }


        /// <summary>
        /// Sends a message with a file attached.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="fileName">Name for the attachment to have</param>
        /// <param name="fileData">Raw byte data from file</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        public static Message SendFile(this DiscordClient client, ulong channelId, string fileName, byte[] fileData, string message = null, bool tts = false)
        {
            HttpClient httpClient = new HttpClient(new HttpClientHandler() { Proxy = client.HttpClient.Proxy != null && client.HttpClient.Proxy.Type == Leaf.xNet.ProxyType.HTTP ? new WebProxy(client.HttpClient.Proxy.Host, client.HttpClient.Proxy.Port) : null });
            httpClient.DefaultRequestHeaders.Add("Authorization", client.Token);

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                {
                    new StringContent(JsonConvert.SerializeObject(new MessageProperties()
                    {
                        Content = message,
                        Tts = tts
                    })),
                    "payload_json"
                },
                { new ByteArrayContent(fileData), "file", fileName }
            };

            return httpClient.PostAsync(client.HttpClient.ApiBaseEndpoint + $"/channels/{channelId}/messages", content).Result
                                    .Content.ReadAsStringAsync().Result.Deserialize<Message>().SetClient(client);
        }


        /// <summary>
        /// Sends a message with a file attached.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filePath">Path to the file you want attached</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        public static Message SendFile(this DiscordClient client, ulong channelId, string filePath, string message = null, bool tts = false)
        {
            return client.SendFile(channelId, new FileInfo(filePath).Name, File.ReadAllBytes(filePath), message, tts);
        }


        /// <summary>
        /// Edits a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the channel</param>
        /// <param name="message">New content of the message</param>
        /// <returns>The edited message</returns>
        public static Message EditMessage(this DiscordClient client, ulong channelId, ulong messageId, string message)
        {
            return client.HttpClient.Patch($"/channels/{channelId}/messages/{messageId}", $"{{\"content\":\"{message}\"}}")
                                .Deserialize<Message>().SetClient(client);
        }


        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void DeleteMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Delete($"/channels/{channelId}/messages/{messageId}");
        }


        /// <summary>
        /// Bulk deletes messages (this is a bot only endpoint)
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messages">IDs of the messages</param>
        public static void DeleteMessages(this DiscordClient client, ulong channelId, List<ulong> messages)
        {
            client.HttpClient.Post($"/channels/{channelId}/messages/bulk-delete", $"{{\"messages\":{JsonConvert.SerializeObject(messages)}}}");
        }
        #endregion


        /// <summary>
        /// Triggers a 'user typing...'
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static void TriggerTyping(this DiscordClient client, ulong channelId)
        {
            var resp = client.HttpClient.Post($"/channels/{channelId}/typing");

            if (resp.ToString().Contains("cooldown"))
                throw new RateLimitException(client, resp.Deserialize<JObject>().GetValue("message_send_cooldown_ms").ToObject<int>());
        }


        #region get channel messages
        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filters">Options for filtering out messages</param>
        public static IReadOnlyList<Message> GetChannelMessages(this DiscordClient client, ulong channelId, MessageFilters filters)
        {
            string parameters = "";
            if (filters.LimitProperty.Set) parameters += $"limit={filters.Limit}&";
            if (filters.BeforeProperty.Set) parameters += $"before={filters.BeforeId}&";
            if (filters.AfterProperty.Set) parameters += $"after={filters.AfterId}&";

            IReadOnlyList<Message> messages = client.HttpClient.Get($"/channels/{channelId}/messages?{parameters}")
                                                          .Deserialize<IReadOnlyList<Message>>().SetClientsInList(client);

            if (filters.UserProperty.Set)
                messages = messages.Where(msg => msg.Author.User.Id == filters.UserId).ToList();

            return messages;
        }


        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="limit">Max amount of messages to receive</param>
        public static IReadOnlyList<Message> GetChannelMessages(this DiscordClient client, ulong channelId, uint limit)
        {
            return client.GetChannelMessages(channelId, new MessageFilters() { Limit = limit });
        }


        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<Message> GetChannelMessages(this DiscordClient client, ulong channelId)
        {
            return client.GetChannelMessages(channelId, new MessageFilters());
        }
        #endregion


        /// <summary>
        /// Gets a message's reactions
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction</param>
        /// <param name="limit">Max amount of reactions to receive</param>
        /// <param name="afterId">Reaction ID to offset from</param>
        public static IReadOnlyList<User> GetMessageReactions(this DiscordClient client, ulong channelId, ulong messageId, string reaction, uint limit = 25, ulong afterId = 0)
        {
            return client.HttpClient.Get($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}?limit={limit}&after={afterId}")
                                .Deserialize<IReadOnlyList<User>>().SetClientsInList(client);
        }


        #region pins
        /// <summary>
        /// Gets a channel's pinned messages
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<Message> GetChannelPinnedMessages(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}/pins")
                                .Deserialize<IReadOnlyList<Message>>().SetClientsInList(client);
        }


        /// <summary>
        /// Pins a message to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void PinChannelMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Put($"/channels/{channelId}/pins/{messageId}");
        }


        /// <summary>
        /// Unpins a message from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void UnpinChannelMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Delete($"/channels/{channelId}/pins/{messageId}");
        }
        #endregion


        public static void AcknowledgeMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Post($"/channels/{channelId}/messages/{messageId}/ack");
        }
    }
}