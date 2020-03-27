using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    public class Role : MinimalRole
    {
        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("color")]
        private uint _color;
        public Color Color
        {
            get { return Color.FromArgb((int)_color); }
            private set { _color = (uint)Color.FromArgb(0, value.R, value.G, value.B).ToArgb(); }
        }


        [JsonProperty("position")]
        public int Position { get; private set; }


        [JsonProperty("hoist")]
        public bool Seperated { get; private set; }


        [JsonProperty("mentionable")]
        public bool Mentionable { get; private set; }


        [JsonProperty("permissions")]
#pragma warning disable IDE0051, IDE1006
        private uint _permissions
        {
            get { return Permissions;  }
            set { Permissions = new Permissions(value); }
        }
#pragma warning restore IDE0051, IDE1006
        public Permissions Permissions { get; private set; }


        /// <summary>
        /// Modifies the role
        /// </summary>
        /// <param name="properties">Options for modifying the role</param>
        public new void Modify(RoleProperties properties)
        {
            Role role = base.Modify(properties);
            Name = role.Name;
            Permissions = role.Permissions;
            Color = role.Color;
            Seperated = role.Seperated;
            Position = role.Position;
        }


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator ulong(Role instance)
        {
            return instance.Id;
        }
    }
}