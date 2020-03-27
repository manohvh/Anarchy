using Newtonsoft.Json;

namespace Discord
{
    public class GroupInvite : Invite
    {
        public GroupInvite()
        {
            OnClientUpdated += (sender, e) => Group.SetClient(Client);
        }


        [JsonProperty("channel")]
        public Group Group { get; private set; }


        public GroupInvite Join()
        {
            return Client.JoinGroup(Code);
        }
    }
}
