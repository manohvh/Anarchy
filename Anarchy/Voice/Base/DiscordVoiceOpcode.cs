namespace Discord.Voice
{
    public enum DiscordVoiceOpcode
    { 
        Identify,
        SelectProtocol,
        Ready,
        Heartbeat,
        SessionDescription,
        Speaking,
        HeartbeatAck,
        Resume,
        Hello,
        Resumed,
        Live = 12,
        ClientDisconnect
    }
}
