using System.Collections.Generic;

namespace Discord
{
    public static class VoiceExtensions
    {
        /// <summary>
        /// Opens a call on the specified channel
        /// </summary>
        /// <param name="channelId">ID of the private channel</param>
        public static void StartCall(this DiscordClient client, ulong channelId)
        {
            client.HttpClient.Post($"/channels/{channelId}/call/ring", $"{{\"recipients\":null}}");
        }

        /// <summary>
        /// Gets all available voice regions
        /// </summary>
        public static IReadOnlyList<VoiceRegion> GetVoiceRegions(this DiscordClient client)
        {
            return client.HttpClient.Get("/voice/regions")
                                .Deserialize<IReadOnlyList<VoiceRegion>>();
        }

        /// <summary>
        /// Mutes the user in the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="unmute">Unmute the user instead of muting them</param>
        public static void MuteGuildMember(this DiscordClient client, ulong guildId, ulong userId, bool unmute = false)
        {
            client.ModifyGuildMember(guildId, userId, new GuildMemberProperties() { Muted = !unmute });
        }


        /// <summary>
        /// Deafenes the user in the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="undeafen">Undeafen the user instead of deafening them</param>
        public static void DeafenGuildMember(this DiscordClient client, ulong guildId, ulong userId, bool undeafen = false)
        {
            client.ModifyGuildMember(guildId, userId, new GuildMemberProperties() { Deafened = !undeafen });
        }
    }
}
