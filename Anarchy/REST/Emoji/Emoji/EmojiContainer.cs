using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    internal class EmojiContainer : Controllable
    {
        public EmojiContainer()
        {
            OnClientUpdated += (sender, e) => Emojis.SetClientsInList(Client);
        }


        private ulong _guildId;
        [JsonProperty("guild_id")]
        public ulong GuildId
        {
            get { return _guildId; }
            set
            {
                _guildId = value;

                foreach (var emoji in Emojis)
                    emoji.GuildId = _guildId;
            }
        }


        [JsonProperty("emojis")]
        public IReadOnlyList<Emoji> Emojis { get; private set; }
    }
}
