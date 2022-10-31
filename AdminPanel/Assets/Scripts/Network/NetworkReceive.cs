using Assets.Cryptography;
using Assets.Scripts.Model;
using HiroNetwork;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

enum ServerPackets
{
    SSearchFiles = 1,
    SSendAdminData = 3,
    SMessageErrors = 4,
    SOnlineUser = 5,
    SWelcomeServer = 6,
    SOnlineOneUser = 7,
    SDisconnectUser = 8,
    SMessageErrorsServer = 9,
}
internal static class NetworkReceive
{   
    internal static void PacketRouter()
    {

        NetworkConfig.socket.PacketId[(int)ServerPackets.SSearchFiles] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_SearchFilesReceive);

        NetworkConfig.socket.PacketId[(int)ServerPackets.SSendAdminData] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_SearchFilesReceive);

        NetworkConfig.socket.PacketId[(int)ServerPackets.SMessageErrors] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_MessageErrors);

        NetworkConfig.socket.PacketId[(int)ServerPackets.SOnlineUser] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_OnlineUser);

        NetworkConfig.socket.PacketId[(int)ServerPackets.SWelcomeServer] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_WelcomeServer);

        NetworkConfig.socket.PacketId[(int)ServerPackets.SOnlineOneUser] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_OnlineOneUser);


        NetworkConfig.socket.PacketId[(int)ServerPackets.SDisconnectUser] =
           new HiroNetwork.Network.Client.Client.DataArgs(Packet_DisconnectUser);

        NetworkConfig.socket.PacketId[(int)ServerPackets.SMessageErrorsServer] =
        new HiroNetwork.Network.Client.Client.DataArgs(Packet_MessageErrorsServer);
    }

    // Получение запрашиваемых данных от пользователя
    private static void Packet_SearchFilesReceive(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        string json_file = buffer.ReadString();
        string json_decrypt = CryptographyData.instance.Decrypt(json_file);

        Debug.Log("Json: " + json_file);
        Debug.Log("Json dec: " + json_decrypt);

        List<FileInfo> json = JsonConvert.DeserializeObject<List<FileInfo>>(json_decrypt);
        UnityMainThreadDispatcher.Instance().Enqueue(ServerMessage.instance.FileSeacrhInfo(json));
    
        buffer.Dispose();
    }

    private static void Packet_MessageErrors(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        string error_message = buffer.ReadString();
        string error_dec = CryptographyData.instance.Decrypt(error_message);

        ServerMessage.instance.TextErrorMessage(error_dec);

        buffer.Dispose();
    }

    private static void Packet_MessageErrorsServer(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        string error_message = buffer.ReadString();
        string error_dec = CryptographyData.instance.Decrypt(error_message);

        ServerMessage.instance.TextErrorMessage(error_dec);

        buffer.Dispose();
    }

    private static void Packet_OnlineUser(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        string json_online_user = buffer.ReadString();
        string json_decrypt = CryptographyData.instance.Decrypt(json_online_user);

        buffer.Dispose();

        List<UserConnection> json = JsonConvert.DeserializeObject<List<UserConnection>>(json_decrypt);

        UnityMainThreadDispatcher.Instance().Enqueue(ServerMessage.instance.TextOnlineUserPrint(json));
    }


    private static void Packet_WelcomeServer(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        int connection_id = buffer.ReadInt32();
        string message = buffer.ReadString();

        NetworkManagers.instance.my_connection_id = connection_id;
        ServerMessage.instance.ConnectionIDText(connection_id);
        ServerMessage.instance.TextErrorMessage(CryptographyData.instance.Decrypt(message));

        buffer.Dispose();
    }


    private static void Packet_OnlineOneUser(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        string json_online_user = buffer.ReadString();
        string json_decrypt = CryptographyData.instance.Decrypt(json_online_user);

        buffer.Dispose();

        UserConnection json = JsonConvert.DeserializeObject<UserConnection>(json_decrypt);

        UnityMainThreadDispatcher.Instance().Enqueue(ServerMessage.instance.TextOnlineOneUserPrint(json));
    }

    private static void Packet_DisconnectUser(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);

        string json_online_user = buffer.ReadString();
        string json_decrypt = CryptographyData.instance.Decrypt(json_online_user);

        buffer.Dispose();

        List<UserConnection> json = JsonConvert.DeserializeObject<List<UserConnection>>(json_decrypt);

        UnityMainThreadDispatcher.Instance().Enqueue(ServerMessage.instance.TextDisconnectedUserPrint(json));
    } 
}
