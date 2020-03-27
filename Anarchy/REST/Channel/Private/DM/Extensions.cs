using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class DMChannelExtensions
    {
        /// <summary>
        /// Gets the account's private channels
        /// </summary>
        /// <returns>A <see cref="IReadOnlyList{DMChannel}"/> containing the client's private channels</returns>
        public static IReadOnlyList<DMChannel> GetPrivateChannels(this DiscordClient client)
        {
            return client.HttpClient.Get($"/users/@me/channels")
                                .DeserializeExArray<DMChannel>().SetClientsInList(client);
        }


        /// <summary>
        /// Gets a direct messaging channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <returns>A <see cref="DMChannel"/> representation of the channel</returns>
        public static DMChannel GetDMChannel(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}")
                    .DeserializeEx<DMChannel>().SetClient(client);
        }


        /// <summary>
        /// Creates a direct messaging channel
        /// </summary>
        /// <param name="recipientId">ID of the user</param>
        /// <returns>The created <see cref="DMChannel"/></returns>
        public static DMChannel CreateDM(this DiscordClient client, ulong recipientId)
        {
            return client.HttpClient.Post($"/users/@me/channels", $"{{\"recipient_id\":\"{recipientId}\"}}")
                    .DeserializeEx<DMChannel>().SetClient(client);
        }


        /// <summary>
        /// Changes the call region (fx. hongkong) for the private channel
        /// </summary>
        /// <param name="channelId">ID of the private channel</param>
        /// <param name="regionId">The region ID (fx. hongkong)</param>
        public static void ChangePrivateCallRegion(this DiscordClient client, ulong channelId, string regionId)
        {
            client.HttpClient.Patch($"/channels/{channelId}/call", $"{{\"region\":\"{regionId}\"}}");
        }
    }
}
