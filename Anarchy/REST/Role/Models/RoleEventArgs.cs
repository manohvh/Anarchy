namespace Discord
{
    public class RoleEventArgs
    {
        public Role Role { get; private set; }

        internal RoleEventArgs(Role role)
        {
            Role = role;
        }


        public override string ToString()
        {
            return Role.ToString();
        }
    }
}
