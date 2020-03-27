using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Discord
{
    public class MinimalRole : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        public ulong GuildId { get; internal set; } // guild id must be set artificially

        public MinimalRole()
        { }


        public MinimalRole(ulong guildId, ulong roleId)
        {
            GuildId = guildId;
            Id = roleId;
        }


        /// <summary>
        /// Modifies the role
        /// </summary>
        /// <param name="properties">Options for modifying the role</param>
        public Role Modify(RoleProperties properties)
        {
            return Client.ModifyGuildRole(GuildId, Id, properties);
        }


        /// <summary>
        /// Deletes the role
        /// </summary>
        public void Delete()
        {
            Client.DeleteGuildRole(GuildId, Id);
        }
    }
}
