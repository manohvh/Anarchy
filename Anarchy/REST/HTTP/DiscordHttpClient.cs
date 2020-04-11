using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Linq;
using Leaf.xNet;
using System.Threading;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordHttpClient
    {
        private readonly DiscordClient _discordClient;

#pragma warning disable IDE0044
        private static JSchema _errorSchema = new JSchemaGenerator().Generate(typeof(DiscordHttpError));
#pragma warning restore IDE0044

        public string AuthToken
        {
            get { return _discordClient.Token; }
        }

        public string UserAgent { get; set; }
        public string SuperProperties { get; set; }
        public string Fingerprint { get; private set; }
        public bool RetryOnRatelimit { get; private set; } = true;
        public string ApiBaseEndpoint { get; set; } = "https://discordapp.com/api/v6";


        public ProxyClient Proxy { get; set; }


        public void SetProxy(ProxyType proxyType, string proxy)
        {
            Proxy = ProxyClient.Parse(proxyType, proxy);
        }


        public DiscordHttpClient(DiscordClient discordClient)
        {
            _discordClient = discordClient;
        }


        public DiscordHttpClient(DiscordClient client, string proxy, ProxyType proxyType) : this(client)
        {
            SetProxy(proxyType, proxy);
        }
        

        public void UpdateFingerprint()
        {
            Fingerprint = Get("/experiments").Deserialize<JObject>().Value<string>("fingerprint");
        }


        /// <summary>
        /// Checks an HTTP response for errors
        /// </summary>
        private void CheckResponse(HttpResponse resp)
        {
            if (resp.StatusCode > HttpStatusCode.NoContent)
            {
                if (resp.StatusCode == (HttpStatusCode)429)
                    throw new RateLimitException(_discordClient, resp.ToString().Deserialize<RateLimit>());
                else if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    if (!resp.Deserialize<JObject>().IsValid(_errorSchema))
                        throw new InvalidParametersException(_discordClient, resp.ToString());
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    throw new InvalidTokenException(AuthToken);

                throw new DiscordHttpException(_discordClient, resp.Deserialize<DiscordHttpError>());
            }
        }


        /// <summary>
        /// Sends an HTTP request and checks for errors
        /// </summary>
        /// <param name="method">HTTP method to use</param>
        /// <param name="endpoint">API endpoint (fx. /users/@me)</param>
        /// <param name="json">JSON content</param>
        private HttpResponse Send(HttpMethod method, string endpoint, object payload = null)
        {
            string json = "{}";

            if (payload != null)
            {
                if (payload.GetType() == typeof(string))
                    json = (string)payload;
                else
                    json = JsonConvert.SerializeObject(payload);
            }

            bool isEndpoint = !endpoint.StartsWith("http");

            if (isEndpoint)
                endpoint = ApiBaseEndpoint + endpoint;

            bool hasData = method == HttpMethod.POST || method == HttpMethod.PATCH || method == HttpMethod.PUT;

            while (true)
            {
                try
                {
                    HttpRequest msg = new HttpRequest
                    {
                        IgnoreProtocolErrors = true
                    };
                    if (hasData)
                        msg.AddHeader(HttpHeader.ContentType, "application/json");
                    if (Fingerprint != null)
                        msg.AddHeader("X-Fingerprint", Fingerprint);
                    msg.AddHeader("X-Super-Properties", SuperProperties);
                    msg.Proxy = Proxy;
                    msg.UserAgent = UserAgent;
                    msg.Authorization = AuthToken;

                    HttpResponse resp = msg.Raw(method, endpoint, hasData ? new StringContent(json) : null);

                    CheckResponse(resp);
                    return resp;
                }
                catch (RateLimitException ex)
                {
                    if (RetryOnRatelimit)
                        Thread.Sleep(ex.RetryAfter);
                    else
                        throw;
                }
            }
        }


        public HttpResponse Get(string endpoint)
        {
            return Send(HttpMethod.GET, endpoint);
        }


        public HttpResponse Post(string endpoint, object payload = null)
        {
            return Send(HttpMethod.POST, endpoint, payload);
        }


        public HttpResponse Delete(string endpoint)
        {
            return Send(HttpMethod.DELETE, endpoint);
        }


        public HttpResponse Put(string endpoint, object payload = null)
        {
            return Send(HttpMethod.PUT, endpoint, payload);
        }


        public HttpResponse Patch(string endpoint, object payload = null)
        {
            return Send(HttpMethod.PATCH, endpoint, payload);
        }
    }
}