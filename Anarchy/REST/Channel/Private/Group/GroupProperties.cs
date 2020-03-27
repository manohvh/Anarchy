using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="Group"/>
    /// </summary>
    public class GroupProperties : ChannelProperties
    {
        private readonly DiscordImage _image = new DiscordImage();

        [JsonProperty("icon")]
#pragma warning disable IDE1006, IDE0051
        private string _icon
        {
            get { return _image; }
        }
#pragma warning restore IDE1006, IDE0051


        private bool IconSet { get; set; }
        [JsonIgnore]
        public Image Icon
        {
            get { return _image.Image; }
            set
            {
                _image.SetImage(value);
                IconSet = true;
            }
        }


        public bool ShouldSerialize_icon()
        {
            return IconSet;
        }
    }
}
