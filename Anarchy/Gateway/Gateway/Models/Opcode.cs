namespace Discord.Gateway
{
    internal enum GatewayOpcode
    {
        Event,
        Heartbeat,
        Identify,
        PresenceChange,
        VoiceStateUpdate,
        Resume = 6,
        Reconnect,
        RequestGuildMembers,
        InvalidSession,
        Connected,
        HeartbeatAck
    }
}