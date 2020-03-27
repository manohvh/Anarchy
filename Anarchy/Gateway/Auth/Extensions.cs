namespace Discord.Gateway
{
    public static class GatewayAuthExtensions
    {
        /// <summary>
        /// Logs intot he gateway
        /// </summary>
        internal static void LoginToGateway(this DiscordSocketClient client)
        {
            var identification = new GatewayIdentification()
            {
                Token = client.Token,
                Properties = client.SuperProperties
            };

            client.Socket.Send(GatewayOpcode.Identify, identification);
        }


        internal static void Resume(this DiscordSocketClient client)
        {
            client.Socket.Send(GatewayOpcode.Resume, new GatewayResume(client));
        }
    }
}
