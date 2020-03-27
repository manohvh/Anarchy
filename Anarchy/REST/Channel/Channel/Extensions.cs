using Newtonsoft.Json;

namespace Discord
{
    public static class ChannelExtensions
    {
        /// <summary>
        /// Gets a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static Channel GetChannel(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}")
                                .DeserializeEx<Channel>().SetClient(client);
        }


        /// <summary>
        /// Modifies a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for modifying the channel</param>
        /// <returns>The modified <see cref="Channel"/></returns>
        public static Channel ModifyChannel(this DiscordClient client, ulong channelId, string name)
        {
            ChannelProperties properties = new ChannelProperties() { Name = name };

            return client.HttpClient.Patch($"/channels/{channelId}",
                    JsonConvert.SerializeObject(properties)).DeserializeEx<Channel>().SetClient(client);
        }


        /// <summary>
        /// Deletes a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <returns>The deleted channel</returns>
        public static Channel DeleteChannel(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Delete($"/channels/{channelId}")
                                .DeserializeEx<Channel>().SetClient(client);
        }
    }
}
