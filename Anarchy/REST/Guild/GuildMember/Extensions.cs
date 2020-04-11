using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public static class GuildMemberExtensions
    {
        /// <summary>
        /// Gets a member of a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        public static GuildMember GetGuildMember(this DiscordClient client, ulong guildId, ulong userId)
        {
            GuildMember member = client.HttpClient.Get($"/guilds/{guildId}/members/{userId}")
                                            .Deserialize<GuildMember>().SetClient(client);
            member.GuildId = guildId;
            return member;
        }


        /// <summary>
        /// Gets a list of members in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="limit">Max amount of members to receive</param>
        /// <param name="afterId">User ID to offset from</param>
        public static IReadOnlyList<GuildMember> GetGuildMembers(this DiscordClient client, ulong guildId, uint limit, ulong afterId = 0)
        {
            IReadOnlyList<GuildMember> members = client.HttpClient.Get($"/guilds/{guildId}/members?limit={limit}&after={afterId}")
                                                            .Deserialize<IReadOnlyList<GuildMember>>().SetClientsInList(client);
            foreach (var member in members) member.GuildId = guildId;
            return members;
        }


        /// <summary>
        /// Gets all members in a guld
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildMember> GetAllGuildMembers(this DiscordClient client, ulong guildId)
        {
            List<GuildMember> members = client.GetGuildMembers(guildId, 1000).ToList();

            while (true)
            {
                IReadOnlyList<GuildMember> newMembers = client.GetGuildMembers(guildId, 1000, members[members.Count - 1].User.Id);
                if (newMembers.Count == 0)
                    break;
                members.AddRange(newMembers);
            }

            return members;
        }


        /// <summary>
        /// Modifies the specified guild member
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="properties">Things to change</param>
        public static void ModifyGuildMember(this DiscordClient client, ulong guildId, ulong userId, GuildMemberProperties properties)
        {
            client.HttpClient.Patch($"/guilds/{guildId}/members/{userId}", properties);
        }


        /// <summary>
        /// Changes a user's nickname in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="nickname">New nickname</param>
        public static void ChangeNickname(this DiscordClient client, ulong guildId, ulong userId, string nickname)
        {
            client.ModifyGuildMember(guildId, userId, new GuildMemberProperties() { Nickname = nickname });
        }
    }
}
