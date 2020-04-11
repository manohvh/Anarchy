﻿using Newtonsoft.Json;

namespace Discord
{
    public class DiscordRegistration
    {
        [JsonProperty("email")]
        public string Email { get; set; }


        [JsonProperty("username")]
        public string Username { get; set; }


        [JsonProperty("password")]
        public string Password { get; set; }


        [JsonProperty("captcha_key")]
        public string CaptchaKey { get; set; }


        [JsonProperty("fingerprint")]
        internal string Fingerprint { get; set; }


        [JsonProperty("consent")]
        private bool _consent = true;
    }
}