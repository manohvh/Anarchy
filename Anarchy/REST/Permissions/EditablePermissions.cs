namespace Discord
{
    public class EditablePermissions : Permissions
    {
        public EditablePermissions() : base() { }
        public EditablePermissions(uint permissions) : base(permissions) { }


        public void Add(Permission permission)
        {
            _value = PermissionCalculator.Add(_value, permission);
        }


        public void Remove(Permission permission)
        {
            _value = PermissionCalculator.Remove(_value, permission);
        }


        public static EditablePermissions operator+(EditablePermissions instance, Permission permission)
        {
            instance.Add(permission);
            return instance;
        }

        
        public static EditablePermissions operator-(EditablePermissions instance, Permission permission)
        {
            instance.Remove(permission);
            return instance;
        }
    }
}
