namespace Discord
{
#pragma warning disable CS0660, CS0661
    public class Permissions
#pragma warning restore CS0660, CS0661
    {
        protected uint _value;

        public Permissions()
        {
            _value = 512;
        }


        public Permissions(uint permissions)
        {
            _value = permissions;
        }


        public bool Has(Permission permission)
        {
            return PermissionCalculator.Has(_value, permission);
        }


        public static implicit operator uint(Permissions instance)
        {
            return instance._value;
        }


        public static bool operator ==(Permissions instance, int permissions)
        {
            return instance._value == permissions;
        }


        public static bool operator !=(Permissions instance, int permissions)
        {
            return instance._value != permissions;
        }
    }
}