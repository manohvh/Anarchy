﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class GuildMemberList : Controllable
    {
        public GuildMemberList()
        {
            OnClientUpdated += (sender, e) => Members.SetClientsInList(Client);
        }


        private ulong _guildId;
        [JsonProperty("guild_id")]
        public ulong GuildId
        {
            get { return _guildId; }
            set
            {
                _guildId = value;
                foreach (var member in Members)
                    member.GuildId = _guildId;
            }
        }


        [JsonProperty("members")]
        public IReadOnlyList<GuildMember> Members { get; private set; }
    }
}