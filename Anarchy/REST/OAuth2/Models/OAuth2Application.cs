using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    public class OAuth2Application : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("icon")]
        public string IconId { get; private set; }


        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("summary")]
        public string Summary { get; private set; }


        [JsonProperty("verify_key")]
        public string VerifyKey { get; private set; }


        [JsonProperty("bot_public")]
        public bool PublicBot { get; private set; }


        [JsonProperty("bot_require_code_grant")]
        public bool RequiresCodeGrant { get; private set; }


        public ApplicationBot AddBot()
        {
            return Client.AddBotToApplication(Id);
        }


        public void Delete()
        {
            Client.DeleteApplication(Id);
        }


        /// <summary>
        /// Gets the application's icon
        /// </summary>
        /// <returns>The icon (returns null if IconId is null)</returns>
        public Image GetAvatar()
        {
            if (IconId == null)
                return null;

            return (Bitmap)new ImageConverter().ConvertFrom(Client.HttpClient.Get($"https://cdn.discordapp.com/app-icons/{Id}/{IconId}.png").ToBytes());
        }


        public static implicit operator ulong(OAuth2Application instance)
        {
            return instance.Id;
        }
    }
}
