using System;
using System.Collections.Generic;
using System.Text;

namespace HiroSystemIO.ServerConfuration
{
    internal class UserManager
    {
        public static Dictionary<int, UserConnection> user_list = new Dictionary<int, UserConnection>();
        public static List<int> id_users = new List<int>();

        public static void CreateUser(int connection_id)
        {
            UserConnection user = new UserConnection
            {
                in_game = true,
                connection_id = connection_id,
                role = RoleUser.User
            };
            user_list.Add(connection_id, user);
            id_users.Add(connection_id);

            for (int i = 0; i < user_list.Count; i++)
            {
                if (user_list[id_users[i]].role == RoleUser.Admin)
                {
                    NetworkSend.OnlineOneUser(id_users[i], ref user);
                }
            }
            NetworkSend.WelcomeServer(connection_id, "Вы были успешно подключены к серверу, ваш ID: " + connection_id);
        }
    }
}
