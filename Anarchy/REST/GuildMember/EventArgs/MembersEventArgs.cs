using Discord.Gateway;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class GuildMembersEventArgs : EventArgs
    {
        public ulong GuildId { get; private set; }
        public IReadOnlyList<GuildMember> Members { get; private set; }

        internal GuildMembersEventArgs(GuildMemberList members)
        {
            GuildId = members.GuildId;
            Members = members.Members;
        }
    }
}
