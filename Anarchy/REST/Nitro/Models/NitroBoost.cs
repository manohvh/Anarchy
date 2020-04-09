using Newtonsoft.Json;
using System;

namespace Discord
{
    public class NitroBoost : Controllable
    {
        public NitroBoost()
        {
            OnClientUpdated += (sender, e) =>
            {
                if (Subscription != null)
                    Subscription.SetClient(Client);
            };
        }

        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("premium_guild_subscription")]
        public DiscordGuildSubscription Subscription { get; private set; }


        [JsonProperty("canceled")]
        public bool Canceled { get; private set; }


        [JsonProperty("cooldown_ends_at")]
#pragma warning disable CS0649
        private string _cooldown;
#pragma warning restore CS0649

        public DateTime? Cooldown
        {
            get
            {
                if (_cooldown == null)
                    return null;
                else
                    return DiscordTimestamp.FromString(_cooldown);
            }
        }


        public static implicit operator ulong(NitroBoost instance)
        {
            return instance.Id;
        }
    }
}
