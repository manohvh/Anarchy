using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    internal class Presence
    {
        [JsonProperty("status")]
        private string _status;
        [JsonIgnore]
        public UserStatus Status
        {
            get
            {
                if (_status == "dnd")
                    return UserStatus.DoNotDisturb;
                else
                    return (UserStatus)Enum.Parse(typeof(UserStatus), _status, true);
            }
            set { _status = value != UserStatus.DoNotDisturb ? value.ToString().ToLower() : "dnd"; }
        }


        [JsonProperty("game")]
        public Activity Activity { get; set; }


        [JsonProperty("since")]
#pragma warning disable CS0169, IDE0051
        private readonly long _since;
#pragma warning restore CS0169, IDE0051


        [JsonProperty("afk")]
        public bool Afk { get; set; }


        public override string ToString()
        {
            return Status.ToString();
        }
    }
}
