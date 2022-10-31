using HiroNetwork;
using HiroSystemIO.Admin;
using HiroSystemIO.Cryptography;
using HiroSystemIO.Models;
using System;
using System.Linq;
using System.Text.Json;

namespace HiroSystemIO.ServerConfuration
{
    enum ClientPackets
    {
        СFoundFiles = 1,
        CErrorsMessages = 2,
        CRegistrationAdmin = 3,
        CAdminSearchData = 4,
        CAdminDeleteData = 5,
    }
    internal static class NetworkReceive
    {
        internal static void PacketRouter()
        {
            NetworkConfiguration.socket.PacketId[(int)ClientPackets.СFoundFiles] = Packet_ClientFoundFiles;
            NetworkConfiguration.socket.PacketId[(int)ClientPackets.CErrorsMessages] = Packet_ErrorMessages;
            NetworkConfiguration.socket.PacketId[(int)ClientPackets.CRegistrationAdmin] = Packet_RegistrationAdmin;
            NetworkConfiguration.socket.PacketId[(int)ClientPackets.CAdminSearchData] = Packet_AdminSearchData;
            NetworkConfiguration.socket.PacketId[(int)ClientPackets.CAdminDeleteData] = Packet_AdminDeleteData;
        }


        // Отправление админу полученных данных от юсера
        private static void Packet_ClientFoundFiles(int connection_id, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);

            string json_files = buffer.ReadString();
            for (int i = 0; i < UserManager.user_list.Count; i++)
            {
                if (UserManager.user_list[UserManager.id_users[i]].role == RoleUser.Admin)
                {
                    NetworkSend.NodeData(UserManager.id_users[i], json_files);
                }
            }

            buffer.Dispose();
        }

        // Отправление данных об ошибках админу от пользователя
        private static void Packet_ErrorMessages(int connection_id, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);

            string error_message = buffer.ReadString();
            for (int i = 0; i < UserManager.user_list.Count; i++)
            {
                if (UserManager.user_list[UserManager.id_users[i]].role == RoleUser.Admin)
                {
                    NetworkSend.ErrorsMessageUser(UserManager.id_users[i], error_message);
                }
            }

            buffer.Dispose();
        }

        // Авторизация админа по токену
        private static void Packet_RegistrationAdmin(int connection_id, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);

            Console.WriteLine(buffer);
            string secret_token = buffer.ReadString();
            string secret_decrypt = CryptographyData.instance.Decrypt(secret_token);

            Console.WriteLine("Полученные данные от " + connection_id + ": " + secret_decrypt);

            if (secret_decrypt == ComponentAuthorization.account_admin.Token && UserManager.user_list[connection_id].role != RoleUser.Admin)
            {
                UserManager.user_list[connection_id].role = RoleUser.Admin;
                buffer.Dispose();


                Console.WriteLine(UserManager.user_list.Count);
                Console.WriteLine("User id: " + UserManager.user_list[connection_id].role);
                Console.WriteLine("Массив пользователей онлайн");
                foreach (var t in UserManager.user_list)
                {
                    Console.WriteLine($"Key:{t.Key}, value:{t.Value.role}");
                }
                NetworkSend.ErrorsMessageServer(connection_id, "Ты успешно получил доступ к админке");
                NetworkSend.OnlineUsers(connection_id);
            }
            else
            {
                buffer.Dispose();
            }
        }

        // Получение от админа запроса поиска данных в дериктории и по паттерну
        // Метод перенаправит письмо тому пользователю, у которого будет идти поиск
        private static void Packet_AdminSearchData(int connection_id, ref byte[] data)
        {

            if (UserManager.user_list[connection_id].role == RoleUser.Admin)
            {
                ByteBuffer buffer = new ByteBuffer(data);
                
                int user_file_search_id = buffer.ReadInt32();


                if (UserManager.id_users.Any(x => x == user_file_search_id))
                {
                    string name_directory = buffer.ReadString();
                    string pattern = buffer.ReadString();

                    NetworkSend.SearchFiles(user_file_search_id, name_directory, pattern);
                }
                else
                {
                    NetworkSend.ErrorsMessageServer(connection_id, $"Пользователя с ID: {user_file_search_id} не существует");
                }
                buffer.Dispose();

            }
            else
            {
                Console.WriteLine("Кто-то пытается получить доступ к этому методу");
            }
        }

        private static void Packet_AdminDeleteData(int connection_id, ref byte[] data)
        {
            if (UserManager.user_list[connection_id].role == RoleUser.Admin)
            {
                ByteBuffer buffer = new ByteBuffer(data);

                int user_file_search_id = buffer.ReadInt32();
                if (UserManager.id_users.Any(x => x == user_file_search_id))
                {
                    string name_directory = buffer.ReadString();
                    NetworkSend.DeleteFiles(user_file_search_id, name_directory);
                } else
                {
                    NetworkSend.ErrorsMessageServer(connection_id, $"Пользователя с ID: {user_file_search_id} не суще-ствует");
                }
                buffer.Dispose();
            } else
            {
                Console.WriteLine("Кто-то пытается получить доступ к этому методу");
            }
        }
    }
}
