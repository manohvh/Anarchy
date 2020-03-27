﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordProfile : Controllable
    {
        public DiscordProfile()
        {
            OnClientUpdated += (sender, e) => 
            {
                User.SetClient(Client);
                ConnectedAccounts.SetClientsInList(Client);
            };
        }


        /// <summary>
        /// Updates the profile's info
        /// </summary>
        public void Update()
        {
            DiscordProfile profile = Client.GetProfile(User.Id);
            User = profile.User;
            MutualGuilds = profile.MutualGuilds;
            ConnectedAccounts = profile.ConnectedAccounts;
        }


        [JsonProperty("user")]
        public User User { get; private set; }


        [JsonProperty("premium_since")]
#pragma warning disable CS0649
        private readonly string _premiumSince;
#pragma warning restore CS0649

        public Nitro Nitro
        {
            get { return new Nitro(_premiumSince); }
        }


        [JsonProperty("mutual_guilds")]
        public IReadOnlyList<MutualGuild> MutualGuilds { get; private set; }

        [JsonProperty("connected_accounts")]
        public IReadOnlyList<ConnectedAccount> ConnectedAccounts { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }


        public static implicit operator ulong(DiscordProfile instance)
        {
            return instance.User.Id;
        }
    }
}
