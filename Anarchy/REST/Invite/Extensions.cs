using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public static class InviteExtensions
    {
        #region management
        /// <summary>
        /// Creates an invite for a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns>The created invite</returns>
        public static Invite CreateInvite(this DiscordClient client, ulong channelId, InviteProperties properties = null)
        {
            if (properties == null)
                properties = new InviteProperties();

            return client.HttpClient.Post($"/channels/{channelId}/invites", JsonConvert.SerializeObject(properties))
                                .Deserialize<Invite>().SetClient(client);
        }


        /// <summary>
        /// Deletes an invite
        /// </summary>
        /// <param name="invCode">The invite's code</param>
        /// <returns>The deleted invite</returns>
        public static Invite DeleteInvite(this DiscordClient client, string invCode)
        {
            return client.HttpClient.Delete($"/invites/{invCode}")
                                .Deserialize<Invite>().SetClient(client);
        }
        #endregion

#pragma warning disable IDE1006
        private static T getInvite<T>(this DiscordClient client, string invCode) where T : Invite
        {
            return client.HttpClient.Get($"/invite/{invCode}?with_counts=true")
                                .DeserializeEx<T>().SetClient(client);
        }
#pragma warning restore IDE1006


        public static Invite GetInvite(this DiscordClient client, string invCode)
        {
            return client.getInvite<Invite>(invCode);
        }


        /// <summary>
        /// Gets a guild's invites
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildInvite> GetGuildInvites(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Get($"/guilds/{guildId}/invites")
                                .DeserializeExArray<GuildInvite>().SetClientsInList(client);
        }
    }
}