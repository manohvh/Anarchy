using Newtonsoft.Json;

namespace Discord
{
    public class Invite : ControllableEx
    {
        public Invite()
        {
            OnClientUpdated += (sender, e) =>
            {
                Inviter.SetClient(Client);
            };
        }


        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("inviter")]
        public User Inviter { get; private set; }


        [JsonProperty("guild")]
#pragma warning disable CS0649
        private readonly BaseGuild _guild;
#pragma warning restore CS0649


        public InviteType Type
        {
            get { return _guild != null ? InviteType.Guild : InviteType.Group; }
        }


        /// <summary>
        /// Deletes the invite
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            Inviter = Client.DeleteInvite(Code).Inviter;
        }
        

        public override string ToString()
        {
            return Code;
        }


        public static implicit operator string(Invite instance)
        {
            return instance.Code;
        }


        public GuildInvite ToGuildInvite()
        {
            if (Type != InviteType.Guild)
                throw new InvalidConvertionException(Client, "Invite is not of a guild");

            return ((GuildInvite)Json.ToObject(typeof(GuildInvite))).SetClient(Client).SetJson(Json);
        }


        public GroupInvite ToGroupInvite()
        {
            if (Type != InviteType.Group)
                throw new InvalidConvertionException(Client, "Invite is not of a group");

            return ((GroupInvite)Json.ToObject(typeof(GroupInvite))).SetClient(Client).SetJson(Json);
        }
    }
}
