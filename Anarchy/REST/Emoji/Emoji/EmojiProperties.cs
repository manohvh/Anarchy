using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="Emoji"/>
    /// </summary>
    public class EmojiProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        #region image
        private readonly DiscordImage _image = new DiscordImage();

        [JsonProperty("image")]
#pragma warning disable IDE1006, IDE0051
        private string _img
        {
            get { return _image; }
        }
#pragma warning restore IDE1006, IDE0051

        [JsonIgnore]
        public Image Image
        {
            get
            {
                return _image.Image;
            }
            set
            {
                _image.SetImage(value);
            }
        }
        #endregion


        public override string ToString()
        {
            return Name;
        }
    }
}