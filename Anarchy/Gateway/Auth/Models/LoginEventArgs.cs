using System.Collections.Generic;

namespace Discord.Gateway
{
    public class LoginEventArgs
    {
        public ClientUser User { get; private set; }
        public IReadOnlyList<LoginGuild> Guilds { get; private set; }
        public IReadOnlyList<DMChannel> PrivateChannels { get; private set; }
        public IReadOnlyList<Relationship> Relationships { get; private set; }


        internal LoginEventArgs(Login login)
        {
            User = login.User;
            Guilds = login.Guilds;
            PrivateChannels = login.PrivateChannels;
            Relationships = login.Relationships;
        }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}
