using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;

namespace Discord
{
    public abstract class BaseGuild : MinimalGuild
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("icon")]
        protected string _iconId;


        public DiscordGuildIconCDNImage Icon
        {
            get
            {
                return new DiscordGuildIconCDNImage(Id, _iconId);
            }
        }


        /// <summary>
        /// Updates the guild's info
        /// </summary>
        public void Update()
        {
            Guild guild = Client.GetGuild(Id);
            Name = guild.Name;
            _iconId = guild.Icon.Hash;
        }


        /// <summary>
        /// Modifies the guild
        /// </summary>
        /// <param name="properties">Options for modifying the guild</param>
        public void Modify(GuildProperties properties)
        {
            if (!properties.IconSet)
                properties.IconId = Icon.Hash;

            Guild guild = Client.ModifyGuild(Id, properties);
            Name = guild.Name;
            _iconId = guild.Icon.Hash;
        }


        /// <summary>
        /// Gets the guild's icon
        /// </summary>
        /// <returns>The guild's icon (returns null if IconId is null)</returns>
        [Obsolete("GetIcon is obsolete. Use Icon.Download() instead", true)]
        public Image GetIcon()
        {
            return null;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
