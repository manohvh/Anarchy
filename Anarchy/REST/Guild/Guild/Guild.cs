using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class Guild : BaseGuild
    {
        public Guild()
        {
            OnClientUpdated += (sender, e) =>
            {
                Roles.SetClientsInList(Client);
                Emojis.SetClientsInList(Client);
            };
        }


        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("region")]
        public string Region { get; private set; }


        [JsonProperty("vanity_url_code")]
        public string VanityInvite { get; private set; }


        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }


        [JsonProperty("default_message_notifications")]
        public GuildDefaultNotifications DefaultNotifications { get; set; }


        [JsonProperty("system_channel_id")]
        public ulong? SystemChannelId { get; private set; }


        [JsonProperty("premium_tier")]
        public GuildPremiumTier PremiumTier { get; private set; }


        [JsonProperty("features")]
        public IReadOnlyList<string> Features { get; private set; }


        private IReadOnlyList<DiscordRole> _roles;
        [JsonProperty("roles")]
        public IReadOnlyList<DiscordRole> Roles
        {
            get { return _roles; }
            private set
            {
                if (value != null)
                    foreach (var role in value) role.GuildId = Id;
                _roles = value;
            }
        }


        private IReadOnlyList<Emoji> _emojis;
        [JsonProperty("emojis")]
        public IReadOnlyList<Emoji> Emojis
        {
            get { return _emojis; }
            private set
            {
                if (value != null)
                    foreach (var reaction in value) reaction.GuildId = Id;
                _emojis = value;
            }
        }


        [JsonProperty("owner_id")]
        public ulong OwnerId { get; private set; }


        [JsonProperty("mfa_level")]
        public bool MfaRequired { get; private set; }


        /// <summary>
        /// Updates the guild's info
        /// </summary>
        public new void Update()
        {
            Guild guild = Client.GetGuild(Id);
            Name = guild.Name;
            _iconId = guild.Icon.Hash;
            Region = guild.Region;
            Roles = guild.Roles;
            Emojis = guild.Emojis;
            VerificationLevel = guild.VerificationLevel;
            DefaultNotifications = guild.DefaultNotifications;
            OwnerId = guild.OwnerId;
            VanityInvite = guild.VanityInvite;
        }


        /// <summary>
        /// Modifies the guild
        /// </summary>
        /// <param name="properties">Options for modifying the guild</param>
        public new void Modify(GuildProperties properties)
        {
            if (!properties.IconSet)
                properties.IconId = Icon.Hash;

            Guild guild = Client.ModifyGuild(Id, properties);
            Name = guild.Name;
            Region = guild.Region;
            _iconId = guild.Icon.Hash;
            VerificationLevel = guild.VerificationLevel;
            DefaultNotifications = guild.DefaultNotifications;
            OwnerId = guild.OwnerId;
            Roles = guild.Roles;
            Emojis = guild.Emojis;
            VanityInvite = guild.VanityInvite;
        }


        /// <summary>
        /// Gets the guild's roles
        /// </summary>
        public override IReadOnlyList<DiscordRole> GetRoles()
        {
            return Roles = base.GetRoles();
        }


        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        public override IReadOnlyList<Emoji> GetEmojis()
        {
            return Emojis = base.GetEmojis();
        }
    }
}