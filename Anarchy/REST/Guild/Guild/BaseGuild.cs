using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http;

namespace Discord
{
    public abstract class BaseGuild : MinimalGuild
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("icon")]
        public string IconId { get; protected set; }


        /// <summary>
        /// Updates the guild's info
        /// </summary>
        public void Update()
        {
            Guild guild = Client.GetGuild(Id);
            Name = guild.Name;
            IconId = guild.IconId;
        }


        /// <summary>
        /// Modifies the guild
        /// </summary>
        /// <param name="properties">Options for modifying the guild</param>
        public void Modify(GuildProperties properties)
        {
            if (!properties.IconSet)
                properties.IconId = IconId;

            Guild guild = Client.ModifyGuild(Id, properties);
            Name = guild.Name;
            IconId = guild.IconId;
        }


        /// <summary>
        /// Gets the guild's icon
        /// </summary>
        /// <returns>The guild's icon (returns null if IconId is null)</returns>
        public Image GetIcon()
        {
            if (IconId == null)
                return null;

#pragma warning disable IDE0067
            return (Bitmap)new ImageConverter()
                        .ConvertFrom(new HttpClient().GetByteArrayAsync($"https://cdn.discordapp.com/icons/{Id}/{IconId}.png").Result);
#pragma warning restore IDE0067
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
