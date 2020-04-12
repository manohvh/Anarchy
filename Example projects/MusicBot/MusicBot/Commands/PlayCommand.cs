using Discord;
using Discord.Commands;
using Discord.Gateway;
using System;
using DotNetTools.SharpGrabber.Internal.Grabbers;
using System.IO;
using System.Net.Http;

namespace MusicBot
{
    [Command("play", "Adds a track to the queue")]
    public class PlayCommand : Command
    {
        public override void Execute(DiscordSocketClient client, string[] args, Message message)
        {
            if (!Program.Sessions.ContainsKey(message.Guild))
                message.Channel.SendMessage("Not connected to a voice channel. Use the join command to play music.");
            else
            {
                if (args.Length > 0)
                {
                    if (args[0].IndexOf("?v=") > -1)
                    {
                        string videoId = args[0].Substring(args[0].IndexOf("?v=") + 3, 11);

                        string basePath = $"Cache/{videoId}";
                        string path = basePath + ".webm";

                        string videoName = null;

                        if (!File.Exists(path))
                        {
                            try
                            {
                                var result = new YouTubeGrabber().GrabAsync(new Uri(args[0])).Result;

                                foreach (var resource in result.Resources)
                                {
                                    if (resource.ResourceUri.Host.Contains("googlevideo.com") && resource.ResourceUri.Query.Contains("mime=audio"))
                                    {
                                        File.WriteAllBytes(path, new HttpClient().GetAsync(resource.ResourceUri.ToString()).Result.Content.ReadAsByteArrayAsync().Result);
                                        File.WriteAllText(basePath + ".txt", result.Title);

                                        videoName = result.Title;

                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                return;
                            }
                        }
                        else
                            videoName = File.ReadAllText(basePath + ".txt");

                        message.Channel.SendMessage($"Added \"{videoName}\" to queue.");

                        Program.Sessions[message.Guild].Queue.Enqueue(new Track(videoName, args[0], path));
                    }
                    else
                        message.Channel.SendMessage($"That appears to not be a valid YouTube video url, <@{message.Author.User.Id}>");
                }
            }
        }
    }
}
