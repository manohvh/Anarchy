using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a universal channel object. This is not specific to any sort of channel
    /// </summary>
    public class Channel : MinimalChannel
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("type")]
        public ChannelType Type { get; private set; }


        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public virtual void Update()
        {
            Channel channel = Client.GetChannel(Id);
            Json = channel.Json;
            Name = channel.Name;
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public new void Modify(string name)
        {
            Channel channel = base.Modify(name);
            Json = channel.Json;
            Name = channel.Name;
        }


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator ulong(Channel instance)
        {
            return instance.Id;
        }


        // Note: the reason the Json == null checks are here is because Anarchy currently does not have a method for getting the children of a JObject which is a list

        
        public GuildChannel ToGuildChannel()
        {
            if (Type == ChannelType.DM || Type == ChannelType.Group)
                throw new InvalidConvertionException(Client, "Channel is not of a guild");

            return ((GuildChannel)Json.ToObject(typeof(GuildChannel))).SetClient(Client).SetJson(Json);
        }


        public TextChannel ToTextChannel()
        {
            if (Type != ChannelType.Text)
                throw new InvalidConvertionException(Client, "Channel is not a guild text channel");

            return ((TextChannel)Json.ToObject(typeof(TextChannel))).SetClient(Client).SetJson(Json);
        }


        public VoiceChannel ToVoiceChannel()
        {
            if (Type == ChannelType.Text)
                throw new InvalidConvertionException(Client, "Channel is not a guild voice channel");

            return ((VoiceChannel)Json.ToObject(typeof(VoiceChannel))).SetClient(Client).SetJson(Json);
        }


        public DMChannel ToDMChannel()
        {
            if (Type != ChannelType.DM)
                throw new InvalidConvertionException(Client, "Channel is not of type: DM");

            return ((DMChannel)Json.ToObject(typeof(DMChannel))).SetClient(Client).SetJson(Json);
        }


        public Group ToGroup()
        {
            if (Type != ChannelType.Group)
                throw new InvalidConvertionException(Client, "Channel is not of type: Group");

            return ((Group)Json.ToObject(typeof(Group))).SetClient(Client).SetJson(Json);
        }
    }
}