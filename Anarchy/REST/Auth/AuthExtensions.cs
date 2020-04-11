using Newtonsoft.Json.Linq;

namespace Discord
{
    public static class AuthExtensions
    {
        public static void LoginToAccount(this DiscordClient client, string email, string password, string captchaKey = null)
        {
            client.Token = client.HttpClient.Post("/auth/login", new LoginRequest()
            {
                Email = email,
                Password = password,
                CaptchaKey = captchaKey
            }).Deserialize<JObject>().Value<string>("token");
        }


        public static void RegisterAccount(this DiscordClient client, DiscordRegistration registration)
        {
            registration.Fingerprint = client.HttpClient.Fingerprint;

            client.Token = client.HttpClient.Post("/auth/register", registration).Deserialize<JObject>().Value<string>("token");
        }
    }
}
