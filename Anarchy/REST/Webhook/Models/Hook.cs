using System.Drawing;
using System.Net.Http;
using Newtonsoft.Json;

namespace Discord.Webhook
{
    public class Hook : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("avatar")]
        public string AvatarId { get; private set; }


        [JsonProperty("user")]
        public User Creator { get; private set; }


        [JsonProperty("token")]
        public string Token { get; private set; }


        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        public Hook()
        {
            OnClientUpdated += (sender, e) => Creator.SetClient(Client);
        }


        /// <summary>
        /// Updates the webhook's info
        /// </summary>
        public void Update()
        {
            Hook hook = Client.GetWebhook(Id, Token);
            Name = hook.Name;
            AvatarId = hook.AvatarId;
            Creator = hook.Creator;
            ChannelId = hook.ChannelId;
        }


        /// <summary>
        /// Modifies the webhook
        /// </summary>
        /// <param name="properties">Options for modifying the webhook</param>
        public void Modify(WebhookProperties properties)
        {
            Hook hook = Client.ModifyChannelWebhook(Id, properties);
            Name = hook.Name;
            AvatarId = hook.AvatarId;
            ChannelId = hook.ChannelId;
        }


        /// <summary>
        /// Deletes the webhook
        /// </summary>
        public void Delete()
        {
            Client.DeleteChannelWebhook(Id, Token);
        }


        /// <summary>
        /// Sends a message through the webhook
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="embed">Embed to include in the message</param>
        /// <param name="profile">Custom Username and Avatar url (both are optional)</param>
        public void SendMessage(string content, Embed embed = null, WebhookProfile profile = null)
        {
            var properties = new WebhookMessageProperties() { Content = content, Embed = embed };

            if (profile != null)
            {
                if (profile.NameProperty.Set)
                    properties.Username = profile.Username;
                if (profile.AvatarProperty.Set)
                    properties.AvatarUrl = profile.AvatarUrl;
            }

            Client.HttpClient.Post($"/webhooks/{Id}/{Token}",
                        JsonConvert.SerializeObject(properties));
        }


        /// <summary>
        /// Gets the webhook's avatar
        /// </summary>
        /// <returns>The avatar (returns null if AvatarId is null)</returns>
        public Image GetAvatar()
        {
            if (AvatarId == null)
                return null;

#pragma warning disable IDE0067
            return (Bitmap)new ImageConverter().ConvertFrom(new HttpClient().GetAsync($"https://cdn.discordapp.com/avatars/{Id}/{AvatarId}.png")
                                                                    .Result.Content.ReadAsByteArrayAsync().Result);
#pragma warning restore IDE0067
        }


        public override string ToString()
        {
            return Token;
        }


        public static implicit operator ulong(Hook instance)
        {
            return instance.Id;
        }
    }
}