using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for changing the account's profile
    /// </summary>
    public class UserProfile
    {
        private readonly Property<string> NameProperty = new Property<string>();
        [JsonProperty("username")]
        public string Username
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeUsername()
        {
            return NameProperty.Set;
        }


        internal Property<uint> DiscriminatorProperty = new Property<uint>();
        [JsonProperty("discriminator")]
        public uint Discriminator
        {
            get { return DiscriminatorProperty; }
            set { DiscriminatorProperty.Value = value; }
        }


        public bool ShouldSerializeDiscriminator()
        {
            return DiscriminatorProperty.Set;
        }


        private readonly Property<string> EmailProperty = new Property<string>();
        [JsonProperty("email")]
        public string Email
        {
            get { return EmailProperty; }
            set { EmailProperty.Value = value; }
        }


        public bool ShouldSerializeEmail()
        {
            return EmailProperty.Set;
        }


        #region avatar
        private readonly DiscordImage _image = new DiscordImage();

        [JsonProperty("avatar")]
#pragma warning disable IDE1006, IDE0051
        private string _avatar
        {
            get { return _image; }
        }
#pragma warning restore IDE1006, IDE0051


        private bool AvatarSet { get; set; }
        [JsonIgnore]
        public Image Avatar
        {
            get { return _image.Image; }
            set
            {
                _image.SetImage(value);

                AvatarSet = true;
            }
        }


        public bool ShouldSerialize_avatar()
        {
            return AvatarSet;
        }
        #endregion


        [JsonProperty("password")]
        public string Password { get; set; }


        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}