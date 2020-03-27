# About
Anarchy is an opensource Discord API wrapper that focuses on making bot programming easy.<br>
iLinked (https://youtube.com/iLinked) is in charge of this project, and has written the wrapper from scratch.<br>

Oh and also: if you're using a bot token make sure to prefix the token with 'Bot '.<br><br>


# Examples

## Logging in
```csharp
// The DiscordClient is the most basic client there is. The gateway is NOT available for this client
DiscordClient client = new DiscordClient();
client.Token = "your token here" //Tokens are evaluated whenever they are put in here. It'll trigger a DiscordHttpException if it's invalid

// Same as DiscordClient, but it has gateway support (to use this you need to include Discord.Gateway)
DiscordSocketClient socketClient = new DiscordSocketClient();
socketClient.Login("your token here"); //This is passed to the Token property (for validation reasons) and then sent to the Gateway 
```

## Joining/leaving a server
```csharp
DiscordClient client = new DiscordClient("your token here");

GuildInvite invite = client.JoinGuild("fortnite");
client.LeaveGuild(invite.Guild.Id);
```

## Sending messages
```csharp
DiscordClient client = new DiscordClient("your token here");

TextChannel channel = client.GetTextChannel(420); //this is ONLY guild text channels, if you wanna get a DM use GetDMChannel(), if you want a group use GetGroup()
channel.TriggerTyping(); //This is optional
channel.SendMessage("Hello, World");
```

## Sending embeds
```csharp
DiscordClient client = new DiscordClient("your token here");

TextChannel channel = client.GetTextChannel(420);

//you can also set a bunch of images. im lazy
EmbedMaker embed = new EmbedMaker();
embed.Title = "this is an embed";
embed.TitleUrl = "https://github.com/iLinked1337/Anarchy";
embed.Description = "sent from Anarchy";
embed.Color = Color.FromArgb(0, 104, 204);
embed.AddField("Anarchy", "is a Discord API wrapper");
embed.Footer.Text = "Made by iLinked";
embed.Author.Name = "iLinked";
embed.Author.Url = "https://youtube.com/iLinked";

channel.SendMessage("hey look it's an embed!", false, embed);
```

## Creating guilds and channels
```csharp
DiscordClient client = new DiscordClient("your token here");

Guild newGuild = client.CreateGuild("cool stuff", Image.FromFile("icon.png"), "eu-central" });
GuildChannel newChannel = newGuild.CreateChannel("my new channel", ChannelType.Text);
```

## Using gateway events
```csharp
static void Main()
{
   // There are obviously a lot more gateway events, i just picked a few
   DiscordSocketClient client = new DiscordSocketClient();
   client.OnLoggedIn += Client_OnLoggedIn;
   client.OnLoggedOut += Client_OnLoggedOut;
   client.OnJoinedGuild += Client_OnJoinedGuild;
   client.OnLeftGuild += Client_OnLeftGuild;
   client.Login("your token here");

   Thread.Sleep(-1);
}

private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
{
   Console.WriteLine($"Logged into {args.User}");
}

private static void Client_OnLoggedOut(DiscordSocketClient client, UserEventArgs args)
{
   Console.WriteLine($"Logged out of {args.User}");
}

private static void Client_OnJoinedGuild(DiscordSocketClient client, GuildEventArgs args)
{
   Console.WriteLine($"Joined guild: {args.Guild}");
}

private static void Client_OnLeftGuild(DiscordSocketClient client, GuildEventArgs args)
{
   Console.WriteLine($"Left guild: {args.Guild}");
}
```

## Changing client settings
```csharp
DiscordClient client = new DiscordClient("your token here");

client.User.ChangeProfile(new UserProfile() { Avatar = Image.FromFile("lol.png") }); //this function is for updating profile related settings

client.User.ChangeSettings(new UserSettings() { Language = Language.EnglishUS }); //this is for changing any other setting
```



## Voice channels
```csharp
static void Main()
{
   DiscordSocketClient client = new DiscordSocketClient();
   client.OnLoggedIn += Client_OnLoggedIn;
   client.Login("your token here");
   
   Thread.Sleep(-1);
}

private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
{
   VoiceChannel channel = client.GetVoiceChannel(420);
   channel.Join(false, true); //deafened
   channel.Leave();
}
```

Example projects can be found in 'Example projects'.
