using System.Collections.Generic;
using System.Threading;

namespace Discord.Gateway
{
    public static class GuildMemberExtensions
    {
        /// <summary>
        /// Requests a member chunk from a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="limit">Max amount of members to receive (<see cref="MemberAmount"/> might help)</param>
        public static void GetGuildMembers(this DiscordSocketClient client, ulong guildId, uint limit = 100)
        {
            var query = new GatewayMemberQuery()
            {
                GuildId = guildId,
                Limit = limit
            };

            client.Socket.Send(GatewayOpcode.RequestGuildMembers, query);
        }


        /// <summary>
        /// Gets all memebers in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildMember> GetAllGuildMembers(this DiscordSocketClient client, ulong guildId)
        {
            List<GuildMember> members = new List<GuildMember>();
            IReadOnlyList<GuildMember> newMembers = new List<GuildMember>();
            client.OnGuildMembersReceived += (c, args) =>
            {
                if (args.GuildId == guildId)
                {
                    newMembers = args.Members;
                    members.AddRange(newMembers);
                }
            };

            client.GetGuildMembers(guildId, MemberAmount.All);

            while (newMembers.Count == MemberAmount.Max || newMembers.Count == 0) Thread.Sleep(20);
            return members;
        }
    }
}
