using Leaf.xNet;

namespace Discord
{
    /// <summary>
    /// Discord client that only supports HTTP
    /// </summary>
    public class DiscordClient
    {
        public DiscordHttpClient HttpClient { get; private set; }
        public SPInformation SuperProperties { get; private set; }
        public ClientUser User { get; internal set; }

        private string _token;
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                string previousToken = Token;

                _token = value;

                try
                {
                    this.GetClientUser();
                }
                catch
                {
                    _token = previousToken;

                    throw;
                }
            }
        }

        public string UserAgent
        {
            get { return SuperProperties.Properties.UserAgent; }
            set
            {
                HttpClient.UserAgent = value;
                SuperProperties.Properties.UserAgent = value;
            }
        }

        public DiscordClient(bool getFingerprint = true)
        {
            HttpClient = new DiscordHttpClient(this);

            SuperProperties = new SPInformation();
            SuperProperties.OnPropertiesUpdated += (sender, args) => 
            {
                HttpClient.SuperProperties = args.Properties.Base64;
                UserAgent = args.Properties.Properties.UserAgent;
            };

            SuperProperties.Base64 = "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiRmlyZWZveCIsImRldmljZSI6IiIsImJyb3dzZXJfdXNlcl9hZ2VudCI6Ik1vemlsbGEvNS4wIChXaW5kb3dzIE5UIDEwLjA7IFdpbjY0OyB4NjQ7IHJ2OjczLjApIEdlY2tvLzIwMTAwMTAxIEZpcmVmb3gvNzMuMCIsImJyb3dzZXJfdmVyc2lvbiI6IjczLjAiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6NTUzOTMsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9";

            if (getFingerprint)
                HttpClient.UpdateFingerprint();
        }

        public DiscordClient(string token, bool getFingerprint = true) : this(getFingerprint)
        {
            Token = token;
        }


        public DiscordClient(string proxy, ProxyType proxyType, bool antiTrack = true) : this(antiTrack)
        {
            HttpClient.SetProxy(proxyType, proxy);
        }


        public DiscordClient(string token, string proxy, ProxyType proxyType, bool antiTrack = true) : this(proxy, proxyType, antiTrack)
        {
            Token = token;
        }
        

        public override string ToString()
        {
            return User.ToString();
        }
    }
}