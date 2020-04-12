using Discord;
using Discord.Voice;
using System.Collections.Generic;
using System.Threading;

namespace MusicBot
{
    class Track
    {
        public Track(string name, string url, string file)
        {
            Name = name;
            Url = url;
            File = file;
        }

        public string Name { get; private set; }
        public string Url { get; private set; }
        public string File { get; private set; }
    }


    class MusicSession
    {
        public MusicSession(ulong guildId)
        {
            _loopQueue = new List<Track>();
            _guildId = guildId;
        }

        private ulong _guildId;
        public DiscordVoiceClient Client { get; set; }
        public VoiceChannel Channel { get; set; }
        public Queue<Track> Queue { get; set; }
        private List<Track> _loopQueue { get; set; }
        public Track CurrentTrack { get; set; }
        public bool Loop { get; set; }

        private bool _stop;

        public void StartQueue()
        {
            while (true)
            {
                if (_stop)
                    return;

                try
                {
                    var track = this.Queue.Dequeue();

                    CurrentTrack = track;

                    this.Client.Speak(DiscordVoiceUtils.ReadFromFile(track.File), 64 * 1024, AudioApplication.Music);

                    if (Loop)
                        _loopQueue.Add(track);
                }
                catch
                {
                    if (Loop && _loopQueue.Count > 0)
                        this.Queue = new Queue<Track>(_loopQueue);
                    else
                        Thread.Sleep(100);
                }
            }
        }

        public void Disconnect()
        {
            Client.Disconnect();

            Program.Sessions.Remove(_guildId);

            _stop = true;
        }
    }
}
