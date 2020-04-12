using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

//if the server is nitro boosted you might encounter problems
namespace GuildDuplicator
{
    //makes it easier to make sure categories get created first (which they need to)
    class OrganizedChannelList
    {
        public OrganizedChannelList(IReadOnlyList<GuildChannel> channels)
        {
            Categories = new List<GuildChannel>();
            TextChannels = new List<TextChannel>();
            VoiceChannels = new List<VoiceChannel>();

            foreach (var channel in channels)
            {
                if (channel.Type == ChannelType.Category)
                    Categories.Add(channel);
                else if (channel.Type == ChannelType.News || channel.Type == ChannelType.Text || channel.Type == ChannelType.Store)
                    TextChannels.Add(channel.ToTextChannel());
                else if (channel.Type == ChannelType.Voice)
                    VoiceChannels.Add(channel.ToVoiceChannel());
            }
        }

        public List<GuildChannel> Categories { get; private set; }
        public List<TextChannel> TextChannels { get; private set; }
        public List<VoiceChannel> VoiceChannels { get; private set; }
    }

    struct RoleDupe
    {
        public DiscordRole OurRole;
        public DiscordRole TargetRole;
    }

    struct CategoryDupe
    {
        public GuildChannel OurCategory;
        public GuildChannel TargetCategory;
    }

    class Program
    {
        static void Main()
        {
            //Create a client with the token
            Console.Write("Token: ");
            DiscordClient client = new DiscordClient(Console.ReadLine());

            //find the guild
            Console.Write($"Guild id: ");
            Guild targetGuild = client.GetGuild(ulong.Parse(Console.ReadLine()));

            Guild ourGuild = DuplicateGuild(client, targetGuild);
            DeleteAllChannels(ourGuild);
            DuplicateChannels(targetGuild, ourGuild, DuplicateRoles(targetGuild, ourGuild));

            Console.WriteLine("Done!");
            Console.ReadLine();
        }


        private static Guild DuplicateGuild(DiscordClient client, Guild guild)
        {
            Console.WriteLine("Duplicating guild...");

            //create the guild and modify it with settings from the target
            Guild ourGuild = client.CreateGuild(guild.Name, guild.Icon.Download(), guild.Region);
            ourGuild.Modify(new GuildProperties() { VerificationLevel = guild.VerificationLevel, DefaultNotifications = guild.DefaultNotifications });

            return ourGuild;
        }


        private static void DeleteAllChannels(Guild guild)
        {
            Console.WriteLine("Deleting default guild channels...");

            //when you create a guild it automatically creates some channels, which we have to delete
            foreach (var channel in guild.GetChannels())
            {
                channel.Delete();
                Console.WriteLine($"Deleted {channel}");
            }
        }


        private static void DuplicateChannels(Guild targetGuild, Guild ourGuild, List<RoleDupe> ourRoles)
        {
            OrganizedChannelList channels = new OrganizedChannelList(targetGuild.GetChannels());

            Console.WriteLine("Duplicating categories...");

            //duplicate category channels
            List<CategoryDupe> ourCategories = new List<CategoryDupe>();
            foreach (var category in channels.Categories)
            {
                //create the category
                GuildChannel ourCategory = ourGuild.CreateChannel(category.Name, ChannelType.Category);
                ourCategory.Modify(new GuildChannelProperties() { Position = category.Position });

                foreach (var overwrite in category.PermissionOverwrites)
                {
                    if (overwrite.Type == PermissionOverwriteType.Member)
                        continue;

                    DiscordPermissionOverwrite ourOverwrite = overwrite;
                    ourOverwrite.Id = ourRoles.First(ro => ro.TargetRole.Id == overwrite.Id).OurRole.Id;
                    ourCategory.AddPermissionOverwrite(ourOverwrite);
                }

                CategoryDupe dupe = new CategoryDupe
                {
                    TargetCategory = category,
                    OurCategory = ourCategory
                };
                ourCategories.Add(dupe);

                Console.WriteLine($"Duplicated {category.Name}");
            }

            Console.WriteLine("Duplicating channels...");

            //duplicate text channels
            foreach (var c in channels.TextChannels)
            {
                TextChannel channel = c.ToTextChannel();

                TextChannel ourChannel = ourGuild.CreateChannel(channel.Name, ChannelType.Text, channel.ParentId != null ? (ulong?)ourCategories.First(ca => ca.TargetCategory.Id == channel.ParentId).OurCategory.Id : null).ToTextChannel();
                ourChannel.Modify(new TextChannelProperties() { Nsfw = channel.Nsfw, Position = channel.Position, Topic = channel.Topic, SlowMode = channel.SlowMode });

                foreach (var overwrite in channel.PermissionOverwrites)
                {
                    if (overwrite.Type == PermissionOverwriteType.Member)
                        continue;

                    DiscordPermissionOverwrite ourOverwrite = overwrite;
                    ourOverwrite.Id = ourRoles.First(ro => ro.TargetRole.Id == overwrite.Id).OurRole.Id;
                    ourChannel.AddPermissionOverwrite(ourOverwrite);
                }

                Console.WriteLine($"Duplicated {channel.Name}");
            }

            //duplicate voice channels
            foreach (var channel in channels.VoiceChannels)
            {
                //create voice channels
                VoiceChannel ourChannel = ourGuild.CreateChannel(channel.Name, ChannelType.Voice, channel.ParentId != null ? (ulong?)ourCategories.First(ca => ca.TargetCategory.Id == channel.ParentId).OurCategory.Id : null).ToVoiceChannel();
                ourChannel.Modify(new VoiceChannelProperties() { Bitrate = channel.Bitrate, Position = channel.Position, UserLimit = channel.UserLimit });

                foreach (var overwrite in channel.PermissionOverwrites)
                {
                    if (overwrite.Type == PermissionOverwriteType.Member)
                        continue;

                    DiscordPermissionOverwrite ourOverwrite = overwrite;
                    ourOverwrite.Id = ourRoles.First(ro => ro.TargetRole.Id == overwrite.Id).OurRole.Id;
                    ourChannel.AddPermissionOverwrite(ourOverwrite);
                }

                Console.WriteLine($"Duplicated {channel.Name}");
            }
        }


        private static List<RoleDupe> DuplicateRoles(Guild targetGuild, Guild ourGuild)
        {
            List<RoleDupe> ourRoles = new List<RoleDupe>();

            Console.WriteLine("Duplicating roles...");

            //duplicate roles
            foreach (var role in targetGuild.GetRoles())
            {
                RoleDupe dupe = new RoleDupe
                {
                    TargetRole = role
                };

                if (role.Name == "@everyone") //we don't wanna create another @everyone role, so we just modify ours instead
                {
                    DiscordRole ourRole = ourGuild.GetRoles().First(r => r.Name == "@everyone");
                    ourRole.Modify(new RoleProperties() { Permissions = new DiscordEditablePermissions(role.Permissions), Color = role.Color, Mentionable = role.Mentionable, Seperated = role.Seperated });
                    dupe.OurRole = ourRole;
                }
                else
                    dupe.OurRole = ourGuild.CreateRole(new RoleProperties() { Name = role.Name, Permissions = new DiscordEditablePermissions(role.Permissions), Color = role.Color, Mentionable = role.Mentionable, Seperated = role.Seperated });
                ourRoles.Add(dupe);

                Console.WriteLine($"Duplicated {role}");
            }

            return ourRoles;
        }
    }
}