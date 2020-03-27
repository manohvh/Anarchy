using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class EmojiExtentions
    {
        #region management
        /// <summary>
        /// Creates an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="properties">Options for creating the emoji</param>
        /// <returns>The created <see cref="Emoji"/></returns>
        public static Emoji CreateGuildEmoji(this DiscordClient client, ulong guildId, EmojiProperties properties)
        {
            Emoji emoji = client.HttpClient.Post($"/guilds/{guildId}/emojis", 
                            JsonConvert.SerializeObject(properties)).Deserialize<Emoji>().SetClient(client);
            emoji.GuildId = guildId;
            return emoji;
        }


        /// <summary>
        /// Modifies an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        /// <param name="name">The emoji's new name</param>
        /// <returns>The moeified <see cref="Emoji"/></returns>
        public static Emoji ModifyGuildEmoji(this DiscordClient client, ulong guildId, ulong emojiId, string name)
        {
            return client.HttpClient.Patch($"/guilds/{guildId}/emojis/{emojiId}", $"{{\"name\":\"{name}\"}}")
                                .Deserialize<Emoji>().SetClient(client);
        }


        /// <summary>
        /// Deletes an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static void DeleteGuildEmoji(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}/emojis/{emojiId}");
        }
        #endregion


        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<Emoji> GetGuildEmojis(this DiscordClient client, ulong guildId)
        {
            var emojis = client.HttpClient.Get($"/guilds/{guildId}/emojis")
                                        .Deserialize<IReadOnlyList<Emoji>>().SetClientsInList(client);
            foreach (var emoji in emojis) emoji.GuildId = guildId;
            return emojis;
        }


        /// <summary>
        /// Gets an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static Emoji GetGuildEmoji(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            Emoji reaction = client.HttpClient.Get($"/guilds/{guildId}/emojis/{emojiId}")
                                        .Deserialize<Emoji>().SetClient(client);
            reaction.GuildId = guildId;
            return reaction;
        }


        /// <summary>
        /// Adds a reaction to a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction to add</param>
        public static void AddMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reaction)
        {
            client.HttpClient.Put($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}/@me");
        }


        /// <summary>
        /// Removes a reaction from a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction to remove</param>
        public static void RemoveMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reaction)
        {
            client.HttpClient.Delete($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}/@me");
        }
    }
}