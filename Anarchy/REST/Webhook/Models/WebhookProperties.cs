using Newtonsoft.Json;
using System.Drawing;

namespace Discord.Webhook
{
    /// <summary>
    /// Options for creating/modifying a webhook
    /// </summary>
    public class WebhookProperties
    {
        private readonly Property<string> NameProperty = new Property<string>();
        [JsonProperty("name")]
        public string Name
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeName()
        {
            return NameProperty.Set;
        }


        #region avatar
        private readonly DiscordImage _image = new DiscordImage();

        [JsonProperty("avatar")]
#pragma warning disable IDE0051, IDE1006
        private string _avatar
        {
            get { return _image; }
        }
#pragma warning restore IDE0051, IDE1006

        private bool AvatarSet { get; set; }
        [JsonIgnore]
        public Image Avatar
        {
            get
            {
                return _image.Image;
            }
            set
            {
                _image.SetImage(value);
                AvatarSet = true;
            }
        }


        public bool ShouldSeriaize_avatar()
        {
            return AvatarSet;
        }
        #endregion


        private readonly Property<ulong> ChannelProperty = new Property<ulong>();
        [JsonProperty("channel_id")]
        public ulong ChannelId
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }


        public bool ShouldSerializeChannelId()
        {
            return ChannelProperty.Set;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
