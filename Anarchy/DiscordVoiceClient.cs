using Discord.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using WebSocketSharp;
using System.Threading.Tasks;
using System.Net.Sockets;
using Leaf.xNet;

namespace Discord.Voice
{
    public class DiscordVoiceClient
    {
        internal WebSocket Socket { get; private set; }
        private UdpClient _udpClient;
        private readonly DiscordSocketClient _client;

        // RTP and Sodium
        private ushort _sequence;
        private uint _timestamp;
        private int _ssrc { get; set; }
        private byte[] _secretKey { get; set; }

        public DiscordVoiceServer Server { get; internal set; }
        public ulong ChannelId { get; internal set; }
        public bool Connected { get; private set; }
        public bool Speaking { get; private set; }
        private bool _stopCurrent { get; set; }


        public delegate void StandardHandler(DiscordVoiceClient client, EventArgs e);

        public event StandardHandler OnConnected;

        public delegate void DisconnectHandler(DiscordVoiceClient client, DiscordVoiceCloseEventArgs error);
        public event DisconnectHandler OnDisconnected;

        public delegate void SpeakingHandler(DiscordVoiceClient client, DiscordVoiceSpeaking speaking);
        public event SpeakingHandler OnUserSpeaking;

        public DiscordVoiceClient(DiscordSocketClient client, DiscordVoiceServer server, ulong channelId)
        {
            _client = client;
            Server = server;
            ChannelId = channelId;
        }

        internal void RemoveHandlers()
        {
            OnConnected = null;
            OnDisconnected = null;
            OnUserSpeaking = null;
        }

        public void Connect()
        {
            if (Socket != null && Socket.IsAlive)
                Disconnect();

            Socket = new WebSocket("wss://" + Server.Server.Split(':')[0] + "?v=4");

            if (_client.HttpClient.Proxy != null)
            {
                if (_client.HttpClient.Proxy.Type == ProxyType.HTTP) //WebSocketSharp only supports HTTP proxies :(
                    Socket.SetProxy($"http://{_client.HttpClient.Proxy.Host}:{_client.HttpClient.Proxy.Port}", "", "");
            }

            Socket.OnClose += (sender, e) => 
            {
                DiscordVoiceCloseEventArgs error = null;
                if (e.Code > 1000)
                    error = new DiscordVoiceCloseEventArgs((DiscordVoiceCloseError)e.Code, e.Reason);

                Connected = false;

                OnDisconnected?.Invoke(this, error);
            };
            Socket.OnMessage += Socket_OnMessage;
            Socket.Connect();
        }

        public void Disconnect()
        {
            Connected = false;

            try
            {
                _client.LeaveVoiceChannel(Server.GuildId);
            }
            catch { }

            if (Socket != null)
                Socket.Close();

            if (_udpClient != null)
                _udpClient.Close();
        }

        public void SetSpeaking(bool speaking)
        {
            Socket.Send(JsonConvert.SerializeObject(new DiscordVoiceRequest<DiscordSpeakingRequest>()
            {
                Opcode = DiscordVoiceOpcode.Speaking,
                Payload = new DiscordSpeakingRequest()
                {
                    Speaking = speaking ? 1 : 0,
                    Delay = 0,
                    SSRC = _ssrc
                }
            }));
        }


        public bool CancelCurrentSpeech()
        {
            if (!Speaking)
                return false;

            _stopCurrent = true;

            while (Speaking) { Thread.Sleep(1); }

            return true;
        }


        private void SendAudioData(OpusEncoder encoder, ref byte[] audio, int offset, ushort seq, uint timestamp)
        {
            byte[] packet = new byte[OpusEncoder.FrameBytes + 12];

            byte[] header = new byte[12];
            header[0] = 0x80;
            header[1] = 0x78;
            header[2] = (byte)(seq >> 8);
            header[3] = (byte)(seq >> 0);
            header[4] = (byte)(timestamp >> 24);
            header[5] = (byte)(timestamp >> 16);
            header[6] = (byte)(timestamp >> 8);
            header[7] = (byte)(timestamp >> 0);
            header[8] = (byte)(_ssrc >> 24);
            header[9] = (byte)(_ssrc >> 16);
            header[10] = (byte)(_ssrc >> 8);
            header[11] = (byte)(_ssrc >> 0);

            Buffer.BlockCopy(header, 0, packet, 0, 12);

            int frameSize = encoder.EncodeFrame(audio, offset, packet, 12);
            int encSize = Sodium.Encrypt(packet, 12, frameSize, packet, 12, header, _secretKey);

            _udpClient.Send(packet, encSize + 12);
        }


        public bool Speak(byte[] audio, uint bitrate, AudioApplication usedFor = AudioApplication.Mixed)
        {
            while (Speaking) { Thread.Sleep(1); }

            Speaking = true;

            var encoder = new OpusEncoder((int)bitrate, usedFor, 0);

            int offset = 0;

            long nextTick = Environment.TickCount;

            try
            {
                while (offset + OpusEncoder.FrameBytes < audio.Length && !_stopCurrent)
                {
                    SetSpeaking(true);

                    long dist = nextTick - Environment.TickCount;

                    if (dist <= 0)
                    {
                        SendAudioData(encoder, ref audio, offset, _sequence, _timestamp);

                        nextTick += OpusEncoder.TimeBetweenFrames;
                        offset += OpusEncoder.FrameBytes;
                        _sequence++;
                        _timestamp += OpusEncoder.FrameSamplesPerChannel;
                    }
                    else
                        Thread.Sleep((int)dist);
                }

                SetSpeaking(false);

                _stopCurrent = false;
                Speaking = false;

                return true;
            }
            catch
            {
                Speaking = false;

                return false;
            }
        }


        private void Socket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            var payload = e.Data.Deserialize<DiscordVoiceResponse>();

            Task.Run(() =>
            {
                switch (payload.Opcode)
                {
                    case DiscordVoiceOpcode.Ready:
                        DiscordVoiceReady ready = payload.Data.ToObject<DiscordVoiceReady>();

                        _ssrc = ready.SSRC;

                        Socket.Send(JsonConvert.SerializeObject(new DiscordVoiceRequest<DiscordVoiceProtocolSelection>()
                        {
                            Opcode = DiscordVoiceOpcode.SelectProtocol,
                            Payload = new DiscordVoiceProtocolSelection()
                            {
                                Protocol = "udp",
                                ProtocolData = new DiscordVoiceProtocolData()
                                {
                                    Host = ready.IP,
                                    Port = ready.Port,
                                    EncryptionMode = "xsalsa20_poly1305"
                                }
                            }
                        }));

                        _udpClient = new UdpClient(ready.IP, ready.Port);
                        break;
                    case DiscordVoiceOpcode.Speaking:
                        OnUserSpeaking?.Invoke(this, payload.Data.ToObject<DiscordVoiceSpeaking>());
                        break;
                    case DiscordVoiceOpcode.SessionDescription:
                        List<byte> why = new List<byte>();

                        foreach (var item in payload.Data.secret_key)
                            why.Add(item.ToObject<byte>());

                        _secretKey = why.ToArray();

                        Connected = true;
                        OnConnected?.Invoke(this, null);
                        break;
                    case DiscordVoiceOpcode.Hello:
                        Socket.Send(JsonConvert.SerializeObject(new DiscordVoiceRequest<DiscordVoiceIdentify>()
                        {
                            Opcode = Connected ? DiscordVoiceOpcode.Resume : DiscordVoiceOpcode.Identify,
                            Payload = new DiscordVoiceIdentify()
                            {
                                GuildId = Server.GuildId,
                                UserId = _client.User.Id,
                                SessionId = _client.SessionId,
                                Token = Server.Token
                            }
                        }));

                        try
                        {
                            while (true)
                            {
                                Socket.Send(JsonConvert.SerializeObject(new DiscordVoiceRequest<long>()
                                {
                                    Opcode = DiscordVoiceOpcode.Heartbeat,
                                    Payload = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                                }));
                                Thread.Sleep(payload.Data.heartbeat_interval);
                            }
                        }
                        catch { }

                        break;
                }
            });
        }
    }
}
