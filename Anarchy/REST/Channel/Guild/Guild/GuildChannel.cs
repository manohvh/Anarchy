using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="Channel"/> specific to any guild channel
    /// </summary>
    public class GuildChannel : Channel
    {   
        [JsonProperty("guild_id")]
        public ulong GuildId { get; internal set; }


        [JsonProperty("position")]
        public uint Position { get; protected set; }
        

        [JsonProperty("parent_id")]
        public ulong? ParentId { get; protected set; }


        [JsonProperty("permission_overwrites")]
        public IReadOnlyList<PermissionOverwrite> PermissionOverwrites { get; protected set; }


        /// <summary>
        /// Updates the channel
        /// </summary>
        public override void Update()
        {
            GuildChannel channel = Client.GetChannel(Id).ToGuildChannel();
            Json = channel.Json;
            Name = channel.Name;
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(GuildChannelProperties properties)
        {
            GuildChannel channel = Client.ModifyGuildChannel(Id, properties);
            Json = channel.Json;
            Name = channel.Name;
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
        }


        /// <summary>
        /// Adds/edits a permission overwrite to a channel
        /// </summary>
        /// <param name="overwrite">The permission overwrite to add/edit</param>
        public void AddPermissionOverwrite(PermissionOverwrite overwrite)
        {
            Client.AddPermissionOverwrite(Id, overwrite);
            List<PermissionOverwrite> overwrites = PermissionOverwrites.ToList();
            if (overwrites.Where(pe => pe.Id == overwrite.Id).Count() > 0)
                overwrites[overwrites.IndexOf(overwrites.First(pe => pe.Id == overwrite.Id))] = overwrite;
            else
                overwrites.Add(overwrite);
            PermissionOverwrites = overwrites;
        }


        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="id">ID of the role or member affected by the overwrite</param>
        public void RemovePermissionOverwrite(ulong id)
        {
            Client.RemovePermissionOverwrite(Id, id);

            try
            {
                List<PermissionOverwrite> overwrites = PermissionOverwrites.ToList();
                overwrites.Remove(PermissionOverwrites.First(pe => pe.Id == id));
                PermissionOverwrites = overwrites;
            }
            catch { }
        }


        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="overwrite">The overwrite to delete</param>
        public void RemovePermissionOverwrite(PermissionOverwrite overwrite)
        {
            RemovePermissionOverwrite(overwrite.Id);
        }
    }
}