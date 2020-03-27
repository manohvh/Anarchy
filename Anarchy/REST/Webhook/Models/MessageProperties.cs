﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Webhook
{
    /// <summary>
    /// Options for sending a message through a webhook
    /// </summary>
    internal class WebhookMessageProperties
    {
        [JsonProperty("content")]
        public string Content { get; set; }


        [JsonProperty("embeds")]
        private List<Embed> _embeds;
        public Embed Embed
        {
            get
            {
                return _embeds == null || _embeds.Count == 0 ? null : _embeds[0];
            }
            set
            {
                if (value == null)
                    _embeds = null;
                else
                    _embeds = new List<Embed>() { value };
            }
        }


        internal Property<string> NameProperty = new Property<string>();
        [JsonProperty("username")]
        public string Username
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeUsername()
        {
            return NameProperty.Set;
        }


        internal Property<string> AvatarProperty = new Property<string>();
        [JsonProperty("avatar_url")]
        public string AvatarUrl
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }


        public bool ShouldSerializeAvatarUrl()
        {
            return AvatarProperty.Set;
        }
    }
}