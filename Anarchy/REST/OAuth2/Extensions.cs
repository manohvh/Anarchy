using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class OAuth2Extensions
    {
        public static IReadOnlyList<AuthorizedApp> GetAuthorizedApps(this DiscordClient client)
        {
            return client.HttpClient.Get($"/oauth2/tokens")
                                .Deserialize<IReadOnlyList<AuthorizedApp>>().SetClientsInList(client);
        }


        /// <summary>
        /// Adds a bot to a server
        /// </summary>
        /// <param name="botId">client_id from the oauth2 url</param>
        /// <param name="guildId">the guild to add the bot to</param>
        /// <param name="permissions">permissions the bot should have</param>
        /// <param name="captchaKey">captcha key used to validate the request</param>
        public static void AuthorizeBot(this DiscordClient client, ulong botId, ulong guildId, DiscordPermissions permissions, string captchaKey)
        {
            client.HttpClient.Post($"/oauth2/authorize?client_id={botId}&scope=bot", JsonConvert.SerializeObject(new DiscordBotAuthorization()
            {
                GuildId = guildId,
                Permissions = permissions,
                CaptchaKey = captchaKey
            }));
        }


        /// <summary>
        /// Creates an OAuth2 application
        /// </summary>
        /// <param name="name">name for the application</param>
        /// <returns></returns>
        public static OAuth2Application CreateApplication(this DiscordClient client, string name)
        {
            return client.HttpClient.Post("/oauth2/applications", $"{{\"name\":\"{name}\"}}")
                                .Deserialize<OAuth2Application>().SetClient(client);
        }


        /// <summary>
        /// Adds a bot to the application
        /// </summary>
        /// <param name="appId">ID of the OAuth2 application</param>
        /// <returns></returns>
        public static ApplicationBot AddBotToApplication(this DiscordClient client, ulong appId)
        {
            return client.HttpClient.Post($"/oauth2/applications/{appId}/bot")
                                .Deserialize<ApplicationBot>();
        }


        /// <summary>
        /// Deletes an OAuth2 application
        /// </summary>
        /// <param name="appId">ID of the application</param>
        public static void DeleteApplication(this DiscordClient client, ulong appId)
        {
            client.HttpClient.Delete($"/oauth2/applications/{appId}");
        }
    }
}
