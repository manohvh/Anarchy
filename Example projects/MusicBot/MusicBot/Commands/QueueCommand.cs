using Discord;
using Discord.Commands;
using Discord.Gateway;

namespace MusicBot
{
    [Command("queue", "Shows all tracks currently in queue")]
    public class QueueCommand : Command
    {
        public override void Execute(DiscordSocketClient client, string[] args, Message message)
        {
            if (!Program.Sessions.ContainsKey(message.Guild))
                message.Channel.SendMessage("Bot is not connected to a voice channel.");
            else
            {
                var embed = new EmbedMaker();
                embed.Title = "Current queue";
                embed.Description = "Showing max 25 results.";
                embed.Color = Program.EmbedColor;
                embed.Footer.Text = Program.EmbedFooter.Text;
                embed.Footer.IconUrl = Program.EmbedFooter.IconUrl;

                embed.AddField(Program.Sessions[message.Guild].CurrentTrack.Name + " (currently playing)", Program.Sessions[message.Guild].CurrentTrack.Url);

                var queue = Program.Sessions[message.Guild].Queue.ToArray();

                for (int i = 0; i < queue.Length; i++)
                {
                    if (i > 23)
                        break;

                    embed.AddField(queue[i].Name, queue[i].Url);
                }

                message.Channel.SendMessage("", false, embed);
            }
        }
    }
}
