using System;

namespace HiroSystemIO.ServerConfuration
{
    [Serializable]
    public class UserConnection
    {
        public int connection_id { get; set; }

        public bool in_game { get; set; }

        public RoleUser role { get; set; }
    }

    [Serializable]
    public enum RoleUser
    {
        User = 0,
        Admin = 1,
    }
}
