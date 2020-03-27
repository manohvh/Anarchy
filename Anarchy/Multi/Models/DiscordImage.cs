using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Discord
{
    /// <summary>
    /// Used for easily converting <see cref="System.Drawing.Image"/>s to Discord's own Base64 type format
    /// </summary>
    public class DiscordImage
    {
        public DiscordImage()
        { }

        public DiscordImage(string base64)
        {
            Base64 = base64;

            MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64.Split(',')[1]));
            Image = Image.FromStream(ms);
            ms.Dispose();
        }

        public string Base64 { get; private set; }

        public Image Image { get; private set; }


        public void SetImage(Image img)
        {
            if (img == null)
                Base64 = null;
            else
            {
                string type;

                if (ImageFormat.Jpeg.Equals(img.RawFormat))
                    type = "jpeg";
                else if (ImageFormat.Png.Equals(img.RawFormat))
                    type = "png";
                else if (ImageFormat.Gif.Equals(img.RawFormat))
                    type = "gif";
                else return;

                Base64 = $"data:image/{type};base64," +
                        Convert.ToBase64String((byte[])new ImageConverter().ConvertTo(img, typeof(byte[])));
                Image = img;
            }
        }


        public static implicit operator string(DiscordImage instance)
        {
            return instance.Base64;
        }
    }
}