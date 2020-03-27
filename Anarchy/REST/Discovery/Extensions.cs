﻿using Discord.Gateway;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class GuildDiscoveryExtensions
    {
        /// <summary>
        /// Queries guilds in Server Discovery
        /// </summary>
        /// <param name="query">The name to search for</param>
        /// <param name="limit">Max amount of guilds to receive</param>
        /// <param name="offset">The offset in the list</param>
        public static IReadOnlyList<DiscoveryGuild> QueryGuilds(this DiscordClient client, string query, int limit = 20, int offset = 0)
        {
            return ((IReadOnlyList<DiscoveryGuild>)JObject.Parse(client.HttpClient.Get($"/discoverable-guilds?query={query}&offset={offset}&limit={limit}").ToString())["guilds"]
                                                            .ToObject(typeof(IReadOnlyList<DiscoveryGuild>))).SetClientsInList(client);
        }


        /// <summary>
        /// Queries guilds in Server Discovery
        /// </summary>
        /// <param name="limit">Max amount of guilds to receive</param>
        /// <param name="offset">The offset in the list</param>
        public static IReadOnlyList<DiscoveryGuild> QueryGuilds(this DiscordClient client, int limit = 20, int offset = 0)
        {
            return client.QueryGuilds("", limit, offset);
        }


        /// <summary>
        /// Activate lurker mode on a guild.
        /// 
        /// Note: this currently does not actually get you into lurk mode on the server because of some weird session_id issues.
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static Guild LurkGuild(this DiscordSocketClient client, ulong guildId)
        {
            return client.HttpClient.Put($"/guilds/{guildId}/members/@me?lurker=true&session_id={client.SessionId}")
                                .Deserialize<Guild>().SetClient(client);
        }


        /// <summary>
        /// Joins a lurkable guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <returns></returns>
        public static Guild JoinGuild(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Put($"/guilds/{guildId}/members/@me?lurker=true")
                                .Deserialize<Guild>().SetClient(client);
        }
    }
}
