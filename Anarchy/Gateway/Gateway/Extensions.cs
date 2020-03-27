using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Discord.Gateway
{
    internal static class GatewayExtensions
    {
        internal static void Send<T>(this WebSocket socket, GatewayOpcode op, T requestData) where T : new()
        {
            socket.Send(JsonConvert.SerializeObject(new GatewayRequest<T>(op) { Data = requestData }));
        }


        /// <summary>
        /// Starts a process that will continuously send heartbeats every <paramref name="interval"/> milliseconds
        /// </summary>
        /// <param name="interval">Amount of time (in milliseconds) to wait between sending heartbeats</param>
        internal static async void StartHeartbeatHandlersAsync(this DiscordSocketClient client, uint interval)
        {
            while (true)
            {
                await Task.Delay((int)interval);
                client.Socket.Send(GatewayOpcode.Heartbeat, client.Sequence);
            }
        }
    }
}