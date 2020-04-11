using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="Guild"/>
    /// </summary>
    internal class GuildCreationProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("region")]
        public string Region { get; set; }


        #region icon
        private readonly DiscordImage _image = new DiscordImage();

        [JsonProperty("icon")]
#pragma warning disable IDE1006, IDE0051
        private string _icon
        {
            get { return _image; }
        }
#pragma warning restore IDE1006, IDE0051

        [JsonIgnore]
        public Image Icon
        {
            get { return _image.Image; }
            set { _image.SetImage(value); }
        }
        #endregion
    }
}