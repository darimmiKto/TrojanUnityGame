using HiroNetwork;
using HiroSystemIO.Cryptography;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HiroSystemIO.ServerConfuration
{
    enum ServerPackets
    {
        SSearchFiles = 1,
        SDeleteFile = 2,
        SSendAdminData = 3,
        SMessageErrors = 4,
        SOnlineUser = 5,
        SWelcomeServer = 6,
        SOnlineOneUser = 7,
        SDisconnectUser = 8,
        SMessageErrorsServer = 9,
    }

    internal class NetworkSend
    {
        // Поиск данных, который попросит админ у пользователя 
        public static void SearchFiles(int user_file_search_id, string name_directory, string pattern)
        {
            if (UserManager.user_list[user_file_search_id].role == RoleUser.User)
            {
                ByteBuffer buffer = new ByteBuffer(4);
                buffer.WriteInt32((int)ServerPackets.SSearchFiles);

                buffer.WriteString(name_directory);
                buffer.WriteString(pattern);

                Console.WriteLine("Отправляет пользователю ID: " + user_file_search_id + " запрос на поиск");
                NetworkConfiguration.socket.SendDataTo(user_file_search_id, buffer.Data, buffer.Head);
                buffer.Dispose();
            }
            else
            {
                Console.WriteLine("Ты пытаешь найти данные у самого себя");
            }
        }

        // Удаление данных, который запросит админ у пользователя
        public static void DeleteFiles(int user_file_search_id, string path_delete_file)
        {
            if (UserManager.user_list[user_file_search_id].role == RoleUser.User)
            {
                ByteBuffer buffer = new ByteBuffer(4);

                buffer.WriteInt32((int)ServerPackets.SDeleteFile);

                buffer.WriteString(path_delete_file);

                NetworkConfiguration.socket.SendDataTo(user_file_search_id, buffer.Data, buffer.Head);
                buffer.Dispose();
            }
            else
            {
                Console.WriteLine("Ты пытаешь удалить данные у самого себя");
            }
        }

        // Отправка полученных данных от пользователя запрашиваемых от админа

        public static void NodeData(int admin_id, string json_files)
        {
            ByteBuffer buffer = new ByteBuffer(4);

            buffer.WriteInt32((int)ServerPackets.SSendAdminData);

            Console.WriteLine("Данные найденные у пользователя " + json_files);
            buffer.WriteString(json_files);

            NetworkConfiguration.socket.SendDataTo(admin_id, buffer.Data, buffer.Head);
            buffer.Dispose();

        }

        // Отправление админу сообщений об ошибке
        public static void ErrorsMessageUser(int admin_id, string error_message)
        {
            ByteBuffer buffer = new ByteBuffer(4);

            buffer.WriteInt32((int)ServerPackets.SMessageErrors);

            buffer.WriteString(error_message);

            NetworkConfiguration.socket.SendDataTo(admin_id, buffer.Data, buffer.Head);
            buffer.Dispose();
        }


        public static void ErrorsMessageServer(int admin_id, string error_message)
        {
            ByteBuffer buffer = new ByteBuffer(4);

            buffer.WriteInt32((int)ServerPackets.SMessageErrors);

            string en = CryptographyData.instance.Encrypt(error_message);
            buffer.WriteString(en);

            NetworkConfiguration.socket.SendDataTo(admin_id, buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void OnlineUsers(int admin_id)
        {
            if (UserManager.user_list[admin_id].role == RoleUser.Admin)
            {
                ByteBuffer buffer = new ByteBuffer(4);

                buffer.WriteInt32((int)ServerPackets.SOnlineUser);


                List<UserConnection> users = new List<UserConnection>();
                for (int i = 0; i < UserManager.user_list.Count; i++)
                {
                    users.Add(UserManager.user_list[UserManager.id_users[i]]);
                }


                string json_online_user = JsonSerializer.Serialize<List<UserConnection>>(users);

                Console.WriteLine(json_online_user);

                buffer.WriteString(CryptographyData.instance.Encrypt(json_online_user));


                NetworkConfiguration.socket.SendDataTo(admin_id, buffer.Data, buffer.Head);
                buffer.Dispose();
            }
            else
            {
                Console.WriteLine("Кто-то пытается получить доступ к онлайну");
            }
        }

        public static void OnlineOneUser(int admin_id, ref UserConnection user)
        {
            if (UserManager.user_list[admin_id].role == RoleUser.Admin)
            {
                ByteBuffer buffer = new ByteBuffer(4);

                buffer.WriteInt32((int)ServerPackets.SOnlineOneUser);

                string json_online_user = JsonSerializer.Serialize<UserConnection>(user);

                buffer.WriteString(CryptographyData.instance.Encrypt(json_online_user));

                NetworkConfiguration.socket.SendDataTo(admin_id, buffer.Data, buffer.Head);
                buffer.Dispose();
            }
            else
            {
                Console.WriteLine("Кто-то пытается получить доступ к онлайну");
            }
        }

        public static void OnlineDisconnectedUser(int admin_id)
        {
            if (UserManager.user_list[admin_id].role == RoleUser.Admin)
            {
                ByteBuffer buffer = new ByteBuffer(4);

                buffer.WriteInt32((int)ServerPackets.SDisconnectUser);

                List<UserConnection> users = new List<UserConnection>();
                for (int i = 0; i < UserManager.user_list.Count; i++)
                {
                    users.Add(UserManager.user_list[UserManager.id_users[i]]);
                }
                string json_online_user = JsonSerializer.Serialize<List<UserConnection>>(users);
                buffer.WriteString(CryptographyData.instance.Encrypt(json_online_user));

                NetworkConfiguration.socket.SendDataTo(admin_id, buffer.Data, buffer.Head);
                buffer.Dispose();
            }
            else
            {
                Console.WriteLine("Кто-то пытается получить доступ к онлайну");
            }
        }

        public static void WelcomeServer(int connection_id, string message)
        {
            ByteBuffer buffer = new ByteBuffer(4);

            buffer.WriteInt32((int)ServerPackets.SWelcomeServer);

            buffer.WriteInt32(connection_id);
            buffer.WriteString(CryptographyData.instance.Encrypt(message));

            Console.WriteLine("Письмо было отправлено юсеру " + connection_id);
            NetworkConfiguration.socket.SendDataTo(connection_id, buffer.Data, buffer.Head);
            buffer.Dispose();
        }
    }
}