using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="Guild"/>
    /// </summary>
    public class GuildProperties
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


        private readonly Property<string> RegionProperty = new Property<string>();
        [JsonProperty("region")]
        public string Region
        {
            get { return RegionProperty; }
            set { RegionProperty.Value = value; }
        }


        public bool ShouldSerializeRegion()
        {
            return RegionProperty.Set;
        }


        #region icon
        private readonly DiscordImage _image = new DiscordImage();
        private string _iconId;
        private bool _useBase64;
        internal bool IconSet;

        [JsonProperty("icon")]
#pragma warning disable IDE1006, IDE0051
        private string _icon
        {
            get { return _useBase64 ? _image.Base64 : _iconId; }
        }
#pragma warning restore IDE1006, IDE0051


        public string IconId
        {
            get { return _iconId; }
            set
            {
                _iconId = value;
                _useBase64 = false;
                IconSet = true;
            }
        }


        [JsonIgnore]
        public Image Icon
        {
            get { return _image.Image; }
            set
            { 
                _image.SetImage(value);
                _useBase64 = true;
                IconSet = true;
            }
        }
        #endregion 


        private readonly Property<ulong> OwnerProperty = new Property<ulong>();
        [JsonProperty("owner_id")]
        public ulong OwnerId
        {
            get { return OwnerProperty; }
            set { OwnerProperty.Value = value; }
        }

        
        public bool ShouldSerializeOwnerId()
        {
            return OwnerProperty.Set;
        }


        private readonly Property<GuildVerificationLevel> VerificationProperty = new Property<GuildVerificationLevel>();
        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel
        {
            get { return VerificationProperty; }
            set { VerificationProperty.Value = value; }
        }


        public bool ShouldSerializeVerificationLevel()
        {
            return VerificationProperty.Set;
        }


        private readonly Property<GuildDefaultNotifications> NotificationsProperty = new Property<GuildDefaultNotifications>();
        [JsonProperty("default_message_notifications")]
        public GuildDefaultNotifications DefaultNotifications
        {
            get { return NotificationsProperty; }
            set { NotificationsProperty.Value = value; }
        }


        public bool ShouldSerializeDefaultNotifications()
        {
            return NotificationsProperty.Set;
        }
    }
}
