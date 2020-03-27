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
            Categories = channels.Where(channel => channel.Type == ChannelType.Category).ToList();
            TextChannels = channels.Where(channel => channel.Type == ChannelType.Text).ToList();
            VoiceChannels = channels.Where(channel => channel.Type == ChannelType.Voice).ToList();
        }

        public List<GuildChannel> Categories { get; private set; }
        public List<GuildChannel> TextChannels { get; private set; }
        public List<GuildChannel> VoiceChannels { get; private set; }
    }

    struct RoleDupe
    {
        public Role OurRole;
        public Role TargetRole;
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
            DeleteAllChannels(client, ourGuild);
            DuplicateChannels(client, targetGuild, ourGuild, DuplicateRoles(client, targetGuild, ourGuild));

            Console.WriteLine("Done!");
            Console.ReadLine();
        }


        private static Guild DuplicateGuild(DiscordClient client, Guild guild)
        {
            Console.WriteLine("Duplicating guild...");

            //create the guild and modify it with settings from the target
            Guild ourGuild = client.CreateGuild(guild.Name, guild.GetIcon(), guild.Region);
            ourGuild.Modify(new GuildProperties() { VerificationLevel = guild.VerificationLevel, DefaultNotifications = guild.DefaultNotifications });

            return ourGuild;
        }


        private static void DeleteAllChannels(DiscordClient client, Guild guild)
        {
            Console.WriteLine("Deleting default guild channels...");

            //when you create a guild it automatically creates some channels, which we have to delete
            foreach (var channel in guild.GetChannels())
            {
                channel.Delete();
                Console.WriteLine($"Deleted {channel}");
                Thread.Sleep(50);
            }
        }


        private static void DuplicateChannels(DiscordClient client, Guild targetGuild, Guild ourGuild, List<RoleDupe> ourRoles)
        {
            OrganizedChannelList channels = new OrganizedChannelList(targetGuild.GetChannels());

            Console.WriteLine("Duplicating categories...");

            //duplicate category channels
            List<CategoryDupe> ourCategories = new List<CategoryDupe>();
            foreach (var c in channels.Categories)
            {
                GuildChannel category = c.ToGuildChannel();

                //create the category
                GuildChannel ourCategory = ourGuild.CreateChannel(category.Name, ChannelType.Category);
                ourCategory.Modify(new GuildChannelProperties() { Position = category.Position });

                foreach (var overwrite in category.PermissionOverwrites)
                {
                    if (overwrite.Type == PermissionOverwriteType.Member)
                        continue;

                    PermissionOverwrite ourOverwrite = overwrite;
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

                Thread.Sleep(50);
            }

            Console.WriteLine("Duplicating channels...");

            //duplicate text channels
            foreach (var c in channels.TextChannels)
            {
                TextChannel channel = c.ToTextChannel();

                TextChannel ourChannel = ourGuild.CreateTextChannel(channel.Name, channel.ParentId != null ? (ulong?)ourCategories.First(ca => ca.TargetCategory.Id == channel.ParentId).OurCategory.Id : null);
                ourChannel.Modify(new TextChannelProperties() { Nsfw = channel.Nsfw, Position = channel.Position, Topic = channel.Topic, SlowMode = channel.SlowMode });

                foreach (var overwrite in channel.PermissionOverwrites)
                {
                    if (overwrite.Type == PermissionOverwriteType.Member)
                        continue;

                    PermissionOverwrite ourOverwrite = overwrite;
                    ourOverwrite.Id = ourRoles.First(ro => ro.TargetRole.Id == overwrite.Id).OurRole.Id;
                    ourChannel.AddPermissionOverwrite(ourOverwrite);
                }

                Console.WriteLine($"Duplicated {channel.Name}");

                Thread.Sleep(50);
            }

            //duplicate voice channels
            foreach (var c in channels.VoiceChannels)
            {
                VoiceChannel channel = c.ToVoiceChannel();

                //create voice channels
                VoiceChannel ourChannel = ourGuild.CreateVoiceChannel(channel.Name, channel.ParentId != null ? (ulong?)ourCategories.First(ca => ca.TargetCategory.Id == channel.ParentId).OurCategory.Id : null);
                ourChannel.Modify(new VoiceChannelProperties() { Bitrate = channel.Bitrate, Position = channel.Position, UserLimit = channel.UserLimit });

                foreach (var overwrite in channel.PermissionOverwrites)
                {
                    if (overwrite.Type == PermissionOverwriteType.Member)
                        continue;

                    PermissionOverwrite ourOverwrite = overwrite;
                    ourOverwrite.Id = ourRoles.First(ro => ro.TargetRole.Id == overwrite.Id).OurRole.Id;
                    ourChannel.AddPermissionOverwrite(ourOverwrite);
                }

                Console.WriteLine($"Duplicated {channel.Name}");

                Thread.Sleep(50);
            }
        }


        private static List<RoleDupe> DuplicateRoles(DiscordClient client, Guild targetGuild, Guild ourGuild)
        {
            List<RoleDupe> ourRoles = new List<RoleDupe>();

            Console.WriteLine("Duplicating roles...");

            //duplicate roles
            foreach (var role in targetGuild.GetRoles())
            {
                RoleDupe dupe = new RoleDupe();
                dupe.TargetRole = role;

                if (role.Name == "@everyone") //we don't wanna create another @everyone role, so we just modify ours instead
                {
                    Role ourRole = ourGuild.GetRoles().First(r => r.Name == "@everyone");
                    ourRole.Modify(new RoleProperties() { Permissions = new EditablePermissions(role.Permissions), Color = role.Color, Mentionable = role.Mentionable, Seperated = role.Seperated });
                    dupe.OurRole = ourRole;
                }
                else
                    dupe.OurRole = ourGuild.CreateRole(new RoleProperties() { Name = role.Name, Permissions = new EditablePermissions(role.Permissions), Color = role.Color, Mentionable = role.Mentionable, Seperated = role.Seperated });
                ourRoles.Add(dupe);

                Console.WriteLine($"Duplicated {role}");

                Thread.Sleep(50);
            }

            return ourRoles;
        }
    }
}