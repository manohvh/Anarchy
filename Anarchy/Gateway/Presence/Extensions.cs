using System;

namespace Discord.Gateway
{
    public static class PresenceExtensions
    {
        /// <summary>
        /// Changes the client's status (online, idle, dnd or invisible)
        /// </summary>
        /// <param name="status">The new status</param>
        public static void SetStatus(this DiscordSocketClient client, UserStatus status)
        {
            var presence = new Presence() { Status = status };

            client.Socket.Send(GatewayOpcode.PresenceChange, presence);
        }


        /// <summary>
        /// Sets the client's activity
        /// </summary>
        public static void SetActivity(this DiscordSocketClient client, Activity activity)
        {
            var presence = new Presence() { Activity = activity };

            client.Socket.Send(GatewayOpcode.PresenceChange, presence);
        }
    }
}