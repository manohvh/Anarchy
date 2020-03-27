using Newtonsoft.Json;

namespace Discord
{
    public class Emoji : PartialEmoji
    {
        public Emoji()
        {
            OnClientUpdated += (sender, e) => Creator.SetClient(Client);
        }


        [JsonProperty("user")]
        public User Creator { get; private set; }


        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId);
            }
        }

        /// <summary>
        /// Updates the emoji's info
        /// </summary>
        public void Update()
        {
            Name = Client.GetGuildEmoji(GuildId, (ulong)Id).Name;
        }


        /// <summary>
        /// Modifies the emoji
        /// </summary>
        /// <param name="name">New name</param>
        public void Modify(string name)
        {
            Name = Client.ModifyGuildEmoji(GuildId, (ulong)Id, name).Name;
        }


        /// <summary>
        /// Deletes the emoji
        /// </summary>
        public void Delete()
        {
            Client.DeleteGuildEmoji(GuildId, (ulong)Id);
        }
    }
}
