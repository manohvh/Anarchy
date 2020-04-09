using Discord.Voice;
using System;
using System.Threading;

namespace Discord.Gateway
{
    public static class VoiceExtensions
    {
        private static void ChangeVoiceState(this DiscordSocketClient client, ulong guildId, ulong? channelId, bool muted = false, bool deafened = false)
        {
            VoiceStateChange state = new VoiceStateChange()
            {
                GuildId =  guildId,
                ChannelId = channelId,
                Muted = muted,
                Deafened = deafened
            };

            client.Socket.Send(GatewayOpcode.VoiceStateUpdate, state);
        }


        /// <summary>
        /// Joins a voice channel.
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="muted">Whether the client will be muted or not</param>
        /// <param name="deafened">Whether the client will be deafened or not</param>
        public static DiscordVoiceClient JoinVoiceChannel(this DiscordSocketClient client, ulong guildId, ulong channelId, bool muted = false, bool deafened = false)
        {
            if (client.ConnectToVoiceChannels)
            {
                DiscordVoiceServer server = null;

                client.OnVoiceServer += (c, result) =>
                {
                    server = result;
                };

                if (client.VoiceClients.ContainsKey(guildId))
                {
                    client.VoiceClients[guildId].Disconnect();
                }

                client.ChangeVoiceState(guildId, channelId, muted, deafened);

                int attempts = 0;

                while (server == null)
                {
                    if (attempts > 10 * 1000)
                        throw new TimeoutException("Gateway did not respond with a server");

                    Thread.Sleep(1);

                    attempts++;
                }

                if (client.VoiceClients.ContainsKey(guildId))
                {
                    client.VoiceClients[guildId].ChannelId = channelId;
                    client.VoiceClients[guildId].Server = server;
                    client.VoiceClients[guildId].RemoveHandlers();

                    return client.VoiceClients[guildId];
                }
                else
                {
                    var vClient = new DiscordVoiceClient(client, server, channelId);
                    client.VoiceClients.Add(guildId, vClient);
                    return vClient;
                }
            }
            else
            {
                client.ChangeVoiceState(guildId, channelId, muted, deafened);

                return null;
            }
        }


        /// <summary>
        /// Leaves a voice channel
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static void LeaveVoiceChannel(this DiscordSocketClient client, ulong guildId)
        {
            client.ChangeVoiceState(guildId, null);
        }
    }
}
