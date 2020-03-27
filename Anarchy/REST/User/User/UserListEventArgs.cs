using System;
using System.Collections.Generic;

namespace Discord
{
    public class UserListEventArgs : EventArgs
    {
        public IReadOnlyList<User> Users { get; private set; }

        internal UserListEventArgs(List<User> users)
        {
            Users = users;
        }
    }
}