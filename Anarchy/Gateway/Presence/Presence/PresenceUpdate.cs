using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord.Gateway
{
    public class PresenceUpdate
    {
        [JsonProperty("user")]
#pragma warning disable CS0649
        private readonly JObject _user;
#pragma warning restore CS0659
        public ulong UserId
        {
            get { return ulong.Parse(_user.GetValue("id").ToString()); }
        }



        [JsonProperty("nick")]
        public string Nickname { get; private set; }


        [JsonProperty("roles")]
        public IReadOnlyList<ulong> Roles { get; private set; }


        [JsonProperty("game")]
        public Activity Activity { get; private set; }


        [JsonProperty("guild_id")]
        public long GuildId { get; private set; }


        [JsonProperty("status")]
        private string _status;
        public UserStatus Status
        {
            get
            {
                return (UserStatus)Enum.Parse(typeof(UserStatus), _status, true);
            }
            set
            {
                _status = value != UserStatus.DoNotDisturb ? value.ToString().ToLower() : "dnd";
            }
        }


        public bool FromGuild
        {
            get { return GuildId == 0; }
        }


        public override string ToString()
        {
            return UserId.ToString();
        }
    }
}
