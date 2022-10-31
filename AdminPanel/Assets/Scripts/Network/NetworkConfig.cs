using HiroNetwork.Network.Client;
using UnityEngine;

internal static class NetworkConfig
{
    private static Client _socket;
    internal static Client socket
    {
        get { return _socket; }
        set
        {
            if (_socket != null)
            {
                _socket.ConnectionSuccess -= Socket_ConnectionSuccess; // успешное подключение
                _socket.ConnectionFailed -= Socket_ConnectionFailed; // не успешное подключение
                _socket.ConnectionLost -= Socket_ConnectionLost; // соединение прервано
                _socket.CrashReport -= Socket_CrashReport; // ошибки сервера
                _socket.TrafficReceived -= Socket_TrafficReceived;
            }
            _socket = value;

            if (_socket != null)
            {
                _socket.ConnectionSuccess += Socket_ConnectionSuccess; 
                _socket.ConnectionFailed += Socket_ConnectionFailed;
                _socket.ConnectionLost += Socket_ConnectionLost; 
                _socket.CrashReport += Socket_CrashReport;
                _socket.TrafficReceived += Socket_TrafficReceived;
            }
        }
    }


    internal static void InitNetwork()
    {
        if (!ReferenceEquals(_socket, null)) return;

        socket = new Client(100);
        NetworkReceive.PacketRouter();
    }

    internal static void ConnectToServer()
    { 
        _socket.Connect("localhost", 5444);
    }

    internal static void DisconnectFromServer()
    {
        _socket.Dispose();
    }

    internal static void Socket_ConnectionSuccess()
    {
        ServerMessage.instance.TextErrorMessage($"Вы успешно подключились к серверу. Идёт авторизация на сервере...");
    }

    internal static void Socket_ConnectionFailed()
    {
        ServerMessage.instance.TextErrorMessage($"Покдлючение к серверу не удалось, что-то пошло не так.");
    }

    internal static void Socket_ConnectionLost()
    {
        ServerMessage.instance.TextErrorMessage($"[ID:{NetworkManagers.instance.my_connection_id}]Соединение с сервером было прервано.");
    }

    internal static void Socket_CrashReport(string reason)
    {
        ServerMessage.instance.TextErrorMessage($"[ID:{NetworkManagers.instance.my_connection_id}]Соединение с сервером было прервано по причине: {reason}");
    }

    internal static void Socket_TrafficReceived(int size, ref byte[] data)
    {
        Debug.Log("Трафик пришёл от сервера, его длина: " + size);
    }
}