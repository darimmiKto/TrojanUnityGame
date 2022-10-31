using HiroNetwork.Network.Server;
using System;

namespace HiroSystemIO.ServerConfuration
{
    internal class NetworkConfiguration
    {
        private static Server _socket;

        internal static Server socket
        {
            get { return _socket; }
            set
            {
                if (_socket != null)
                {
                    _socket.ConnectionReceived -= Socket_ConnectionReceived;
                    _socket.ConnectionLost -= Socket_ConnectionLost;
                    _socket.CrashReport -= Socket_CrashReport;
                    _socket.TrafficReceived -= Socket_TrafficReceived;
                }
                _socket = value;

                if (_socket != null)
                {
                    _socket.ConnectionReceived += Socket_ConnectionReceived;
                    _socket.ConnectionLost += Socket_ConnectionLost;
                    _socket.CrashReport += Socket_CrashReport;
                    _socket.TrafficReceived += Socket_TrafficReceived;
                }
            }
        }

        internal static void InitNetwork()
        {
            try
            {
                if (!(socket == null)) return;

                socket = new Server(1000)
                {
                    BufferLimit = 2048000,
                    PacketAcceptLimit = 100,
                    PacketDisconnectCount = 150
                };

                NetworkReceive.PacketRouter();
            }
            catch (Exception)
            {
                Console.WriteLine("Пользователь не смог подключиться к серверу");
            }


        }

        internal static void Socket_ConnectionReceived(int connection_id)
        {
            Console.WriteLine($"Пользователь подключился. Его данные [ID: {connection_id}], [IP: {socket.ClientIp(connection_id)}], [IPv4 server: {socket.GetIPv4()}], [IS_CONNECTED: {socket.IsConnected(connection_id)}]");
            UserManager.CreateUser(connection_id);
        }


        internal static void Socket_ConnectionLost(int connection_id)
        {
            Console.WriteLine("Пользователь номер[" + connection_id + "], был отключён от серверу.");
            UserManager.user_list.Remove(connection_id);
            UserManager.id_users.Remove(connection_id);
            for (int i = 0; i < UserManager.user_list.Count; i++)
            {
                if (UserManager.user_list[UserManager.id_users[i]].role == RoleUser.Admin)
                {
                    NetworkSend.OnlineDisconnectedUser(UserManager.id_users[i]);
                }
            }
        }

        internal static void Socket_CrashReport(int connection_id, string reason)
        {
            Console.WriteLine("Пользователь номер[" + connection_id + "], был отключён от серверу, по причине: " + reason + ".");
        }

        internal static void Socket_TrafficReceived(int size, ref byte[] data)
        {
            Console.WriteLine("Трафик пришёл от сервера, его длина: " + size);
        }
    }
}
