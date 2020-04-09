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
        public static void RequestGuildMembers(this DiscordSocketClient client, ulong guildId, uint limit = 100)
        {
            var query = new GatewayMemberQuery()
            {
                GuildId = guildId,
                Limit = limit
            };

            client.Socket.Send(GatewayOpcode.RequestGuildMembers, query);
        }


        /// <summary>
        /// Requests a member chunk from a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="chunks">Ranges to grab</param>
        public static void RequestGuildMembersNew(this DiscordSocketClient client, ulong guildId, ulong channelId, int[][] chunks)
        {
            var query = new GatewayUserMemberQuery()
            {
                GuildId = guildId
            };

            query.Channels.Add(channelId, chunks);

            client.Socket.Send(GatewayOpcode.RequestGuildMembersUser, query);
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

            client.RequestGuildMembers(guildId, MemberAmount.All);

            while (newMembers.Count == MemberAmount.Max || newMembers.Count == 0) Thread.Sleep(20);

            return members;
        }


        public static IReadOnlyList<GuildMember> GetAllGuildMembersNew(this DiscordSocketClient client, ulong guildId, ulong channelId)
        {
            List<GuildMember> members = new List<GuildMember>();

            int lastOffset = 100;

            bool done = false;

            client.OnGuildMembersReceived += (c, args) =>
            {
                if (args.GuildId == guildId && args.Sync.Value)
                {
                    members.AddRange(args.Members);

                    if (args.Members.Count > 0)
                    {
                        int offset = lastOffset;
                        int limit = lastOffset + 99;

                        lastOffset = limit + 1;

                        client.RequestGuildMembersNew(guildId, channelId, new int[][] { new int[] { offset, limit } });
                    }
                    else
                        done = true;
                }
            };

            client.RequestGuildMembersNew(guildId, channelId, new int[][] { new int[] { 0, 99 } });

            while (!done) { Thread.Sleep(1); };

            return members;
        }
    }
}
