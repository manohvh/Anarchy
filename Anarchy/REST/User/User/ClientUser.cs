using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Account user
    /// </summary>
    public class ClientUser : User
    {
        [JsonProperty("token")]
        internal string Token { get; private set; }


        [JsonProperty("email")]
        public string Email { get; private set; }


        [JsonProperty("verified")]
        public bool EmailVerified { get; private set; }


        [JsonProperty("mfa_enabled")]
        public bool TwoFactorAuth { get; private set; }


        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }


        [JsonProperty("locale")]
        public string Language { get; private set; }


        [JsonProperty("premium_type")]
        public NitroType Nitro { get; private set; }


        /// <summary>
        /// Updates the user's info
        /// </summary>
        public override void Update()
        {
            Client.GetClientUser();
        }


        internal void Update(User user)
        {
            Username = user.Username;
            Discriminator = user.Discriminator;
            AvatarId = user.AvatarId;
            Badges = user.Badges;
        }


        /// <summary>
        /// Changes the user's profile
        /// </summary>
        /// <param name="settings">Options for changing the profile</param>
        public void ChangeProfile(UserProfile settings)
        {
            if (settings.Email == null)
                settings.Email = Email;
            if (!settings.DiscriminatorProperty.Set)
                settings.Discriminator = Discriminator;
            if (settings.Username == null)
                settings.Username = Username;

            ClientUser user = Client.HttpClient.Patch("/users/@me", JsonConvert.SerializeObject(settings)).Deserialize<ClientUser>();
            Email = user.Email;
            EmailVerified = user.EmailVerified;
            Username = user.Username;
            Discriminator = user.Discriminator;
            TwoFactorAuth = user.TwoFactorAuth;
            Nitro = user.Nitro;

            if (user.Token != null)
                Client.Token = user.Token;
        }


        /// <summary>
        /// Gets the users settings
        /// </summary>
        public UserSettings GetSettings()
        {
            return Client.HttpClient.Get("/users/@me/settings")
                                .Deserialize<UserSettings>();
        }


        /// <summary>
        /// Changes the user's settings
        /// </summary>
        public UserSettings ChangeSettings(UserSettings settings)
        {
            return Client.HttpClient.Patch("/users/@me/settings", JsonConvert.SerializeObject(settings))
                                .Deserialize<UserSettings>();
        }


        /// <summary>
        /// Sends a password reset request to the client's email
        /// </summary>
        public void RequestPasswordReset()
        {
            Client.HttpClient.Post("/auth/forgot", $"{{\"email\":\"{Email}\"}}");
        }


        /// <summary>
        /// Deletes the account
        /// </summary>
        /// <param name="password">The account's password</param>
        public void Delete(string password)
        {
            Client.HttpClient.Post("/users/@me/delete", $"{{\"password\":\"{password}\"}}");
        }


        /// <summary>
        /// Disables the account
        /// </summary>
        /// <param name="password">The account's password</param>
        public void Disable(string password)
        {
            Client.HttpClient.Post("/users/@me/disable", $"{{\"password\":\"{password}\"}}");
        }


        /// <summary>
        /// Sets the account's hypesquad
        /// </summary>
        public void SetHypesquad(Hypesquad house)
        {
            if (house == Hypesquad.None)
                Client.HttpClient.Delete("/hypesquad/online");
            else
                Client.HttpClient.Post("/hypesquad/online", $"{{\"house_id\":{(int)house}}}");
        }
    }
}
