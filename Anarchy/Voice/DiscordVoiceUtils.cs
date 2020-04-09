﻿using System.Diagnostics;
using System.IO;

namespace Discord.Voice
{
    public static class DiscordVoiceUtils
    {
        public static byte[] ReadFromFile(string path)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });

            MemoryStream memStream = new MemoryStream();

            process.StandardOutput.BaseStream.CopyTo(memStream);
            return memStream.ToArray();
        }
    }
}
