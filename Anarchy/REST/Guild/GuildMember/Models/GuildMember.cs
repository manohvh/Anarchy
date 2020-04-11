using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class GuildMember : Controllable
    {
        public GuildMember()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }


        [JsonProperty("guild_id")]
        internal ulong GuildId { get; set; }


        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId);
            }
            internal set
            {
                GuildId = value.Id;
            }
        }


        [JsonProperty("user")]
        public User User { get; internal set; }


        [JsonProperty("nick")]
        public string Nickname { get; internal set; }


        [JsonProperty("roles")]
        private IReadOnlyList<ulong> _roles;

        public IReadOnlyList<MinimalRole> Roles
        {
            get
            {
                var roles = new List<MinimalRole>();

                foreach (var role in _roles)
                    roles.Add(new MinimalRole(GuildId, role).SetClient(Client));

                return roles;
            }
        }


        [JsonProperty("joined_at")]
#pragma warning disable CS0649
        private string _joinedAt;
        public DateTime? JoinedAt
        {
            get
            {
                if (_joinedAt == null)
                    return null;

                return DiscordTimestamp.FromString(_joinedAt);
            }
            internal set
            {
                if (value == null)
                    _joinedAt = null;
                else
                    _joinedAt = DiscordTimestamp.ToString(value.Value);
            }
        }
#pragma warning restore CS0649


        [JsonProperty("mute")]
        public bool Muted { get; private set; }

        
        [JsonProperty("deaf")]
        public bool Deafened { get; private set; }


        /// <summary>
        /// Updates the member's information
        /// </summary>
        public void Update()
        {
            GuildMember member = Client.GetGuildMember(GuildId, User.Id);
            User = member.User;
            Nickname = member.Nickname;
            _roles = member._roles;
            Muted = member.Muted;
            Deafened = member.Deafened;
        }


        /// <summary>
        /// Modifies the specified guild member
        /// </summary>
        /// <param name="properties">Things to change</param>
        public void Modify(GuildMemberProperties properties)
        {
            Client.ModifyGuildMember(GuildId, User.Id, properties);
        }

        /// <summary>
        /// Mutes the user in the specified guild
        /// </summary>
        /// <param name="unmute">Unmute the user instead of muting them</param>
        public void Mute(bool unmute = false)
        {
            Client.ModifyGuildMember(GuildId, User.Id, new GuildMemberProperties() { Muted = !unmute });
        }


        /// <summary>
        /// Deafenes the user in the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="undeafen">Undeafen the user instead of deafening them</param>
        public void Deafen(bool undeafen = false)
        {
            Client.ModifyGuildMember(GuildId, User.Id, new GuildMemberProperties() { Deafened = !undeafen });
        }


        /// <summary>
        /// Sets the member's roles
        /// </summary>
        public void SetRoles(List<ulong> roles)
        {
            Client.SetGuildMemberRoles(GuildId, User.Id, roles);
        }


        /// <summary>
        /// Sets the member's roles
        /// </summary>
        public void SetRoles(List<DiscordRole> roles)
        {
            List<ulong> ids = new List<ulong>();
            foreach (var role in roles)
                ids.Add(role.Id);

            SetRoles(ids);
        }


        /// <summary>
        /// Adds a role to the guild member
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        public void AddRole(ulong roleId)
        {
            Client.AddRoleToUser(GuildId, roleId, User.Id);
        }


        /// <summary>
        /// Adds a role to the guild member
        /// </summary>
        public void AddRole(DiscordRole role)
        {
            AddRole(role.Id);
        }


        /// <summary>
        /// Removes a role from the guild member
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        public void RemoveRole(ulong roleId)
        {
            Client.RemoveRoleFromUser(GuildId, roleId, User.Id);
        }


        /// <summary>
        /// Removes a role from the guild member
        /// </summary>
        public void RemoveRole(DiscordRole role)
        {
            RemoveRole(role.Id);
        }



        /// <summary>
        /// Kicks the member from the guild
        /// </summary>
        public void Kick()
        {
            Client.KickGuildMember(GuildId, User.Id);
        }


        /// <summary>
        /// Bans the member from the guild
        /// </summary>
        /// <param name="reason">The reason for banning the user</param>
        /// <param name="deleteMessageDays">Amount of days to purge messages (max is 7)</param>
        public void Ban(string reason = null, uint deleteMessageDays = 0)
        {
            Client.BanGuildMember(GuildId, User.Id, reason, deleteMessageDays);
        }


        /// <summary>
        /// Unbans the user from the guild
        /// </summary>
        public void Unban()
        {
            Client.UnbanGuildMember(GuildId, User.Id);
        }


        public override string ToString()
        {
            return User.ToString();
        }


        public static implicit operator ulong(GuildMember instance)
        {
            return instance.User.Id;
        }
    }
}