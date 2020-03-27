using Newtonsoft.Json;
using Discord.Gateway;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="Channel"/> specific to guild voice channels
    /// </summary>
    public class VoiceChannel : GuildChannel
    {
        [JsonProperty("bitrate")]
        public uint Bitrate { get; private set; }


        [JsonProperty("user_limit")]
        public uint UserLimit { get; private set; }


        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public override void Update()
        {
            VoiceChannel channel = Client.GetChannel(Id).ToVoiceChannel();
            Json = channel.Json;
            Name = channel.Name;
            Bitrate = channel.Bitrate;
            UserLimit = channel.UserLimit;
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(VoiceChannelProperties properties)
        {
            VoiceChannel channel = Client.ModifyVoiceChannel(Id, properties);
            Name = channel.Name;
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
            Bitrate = channel.Bitrate;
            UserLimit = channel.UserLimit;
        }


        public void Join(bool muted = false, bool deafened = false)
        {
            if (Client.GetType() == typeof(DiscordSocketClient))
                ((DiscordSocketClient)Client).JoinVoiceChannel(GuildId, Id, muted, deafened);
        }


        public void Leave()
        {
            if (Client.GetType() == typeof(DiscordSocketClient))
                ((DiscordSocketClient)Client).LeaveVoiceChannel(GuildId);
        }


        /// <summary>
        /// Creates an invite
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns></returns>
        public Invite CreateInvite(InviteProperties properties = null)
        {
            return Client.CreateInvite(Id, properties);
        }
    }
}