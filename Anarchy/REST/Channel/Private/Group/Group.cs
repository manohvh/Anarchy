using System.Drawing;
using System.Net.Http;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="Channel"/> specific to groups
    /// </summary>
    public class Group : DMChannel
    {
        [JsonProperty("icon")]
        public string IconId { get; private set; }


        [JsonProperty("owner_id")]
        public ulong OwnerId { get; private set; }


        /// <summary>
        /// Updates the group's info
        /// </summary>
        public override void Update()
        {
            Group group = Client.GetGroup(Id);
            Json = group.Json;
            Name = group.Name;
            IconId = group.IconId;
            OwnerId = group.OwnerId;
            Recipients = group.Recipients;
        }


        /// <summary>
        /// Modifies the group
        /// </summary>
        /// <param name="properties">Options for modifying the group</param>
        public void Modify(GroupProperties properties)
        {
            Group group = Client.ModifyGroup(Id, properties);
            Json = group.Json;
            Name = group.Name;
            IconId = group.IconId;
            OwnerId = group.OwnerId;
            Recipients = group.Recipients;
        }


        /// <summary>
        /// Adds a recipient to the group
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void AddRecipient(ulong userId)
        {
            Client.AddUserToGroup(Id, userId);
        }


        /// <summary>
        /// Adds a recipient to the group
        /// </summary>
        public void AddRecipient(User user)
        {
            AddRecipient(user.Id);
        }


        /// <summary>
        /// Removes a user from the group
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public void RemoveRecipient(ulong userId)
        {
            Client.RemoveUserFromGroup(Id, userId);
        }


        /// <summary>
        /// Removes a user from the group
        /// </summary>
        public void RemoveRecipient(User user)
        {
            RemoveRecipient(user.Id);
        }


        public new void Leave()
        {
            Group group = Client.DeleteChannel(Id).ToGroup();

            Name = group.Name;
            Recipients = group.Recipients;
            IconId = group.IconId;
            OwnerId = group.OwnerId;
        }


        /// <summary>
        /// Creates an invite
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        public Invite CreateInvite(InviteProperties properties = null)
        {
            return Client.CreateInvite(Id, properties);
        }


        /// <summary>
        /// Gets the group's icon
        /// </summary>
        /// <returns>The icon (null if IconId is null)</returns>
        public Image GetIcon()
        {
            if (IconId == null)
                return null;

            return (Bitmap)new ImageConverter()
                        .ConvertFrom(new HttpClient().GetByteArrayAsync($"https://cdn.discordapp.com/icons/{Id}/{IconId}.png").Result);
        }
    }
}