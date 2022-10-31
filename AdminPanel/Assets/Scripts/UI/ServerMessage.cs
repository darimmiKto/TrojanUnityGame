using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Assets.Scripts.Model;
using System.Collections.Generic;
using System.IO;

public class ServerMessage : MonoBehaviour
{
    public static ServerMessage instance;

    public TextMeshProUGUI connection_id;
    public TextMeshProUGUI text_user_online;

    public Text input_patch;
    public Text input_patch_save_data;
    public Text input_pattern;
    public Text input_token;
    public Text input_id_user;

    public TextMeshProUGUI message_server;

    private StringBuilder builder_message_server = new StringBuilder();
    private StringBuilder builder_message_user_online = new StringBuilder();

    public void Start()
    {
        instance = this;
    }
    public void TextErrorMessage(string error_message)
    {
        builder_message_server.AppendLine($"<color=#9C00FF>[{DateTime.Now}]</color> {error_message}");
        UnityMainThreadDispatcher.Instance().Enqueue(TextPrint());
    }

    public IEnumerator TextPrint()
    {
        message_server.text = builder_message_server.ToString();
        yield return null;
    }

    public IEnumerator FileSeacrhInfo(List<FileInfo> files)
    {
        for (int i = 0; i < files.Count; i++)
        {
            builder_message_server.AppendLine($"<color=#9C00FF>[{DateTime.Now}]</color>[File num {i + 1}]");
            builder_message_server.AppendLine($"\t[Name]: {files[i].Name}]");
            builder_message_server.AppendLine($"\t[FullName]: {files[i].FullName}]");
            builder_message_server.AppendLine($"\t[CreationTime]: {files[i].CreationTime}]");
            builder_message_server.AppendLine($"\t[DirectoryName]: {files[i].DirectoryName}]");
            builder_message_server.AppendLine($"\t[Attributes]: {files[i].Attributes}]");
            builder_message_server.AppendLine($"\t[Length]: {files[i].Length / 1024f / 1024f}mb");
        }
        message_server.text = builder_message_server.ToString();
        yield return null;
    }


    public IEnumerator TextOnlineUserPrint(List<UserConnection> users)
    {
        foreach (var user in users)
        {
            builder_message_user_online.AppendLine($"<color=#9C00FF>[{DateTime.Now}]</color> ID: {user.connection_id}, Role: {user.role}");//
        }
        text_user_online.text = builder_message_user_online.ToString();
        yield return null;
    }

    public IEnumerator TextOnlineOneUserPrint(UserConnection user)
    {
        builder_message_user_online.AppendLine($"<color=#9C00FF>[{DateTime.Now}]</color> ID: {user.connection_id}, Role: {user.role}");
        text_user_online.text = builder_message_user_online.ToString();
        yield return null;
    }

    public IEnumerator TextDisconnectedUserPrint(List<UserConnection> users)
    {
        builder_message_user_online.Clear();
        Console.WriteLine("Builder: " + builder_message_user_online.ToString());
        foreach (var user in users)
        {
            builder_message_user_online.AppendLine($"<color=#9C00FF>[{DateTime.Now}]</color> ID: {user.connection_id}, Role: {user.role}");//
        }
        text_user_online.text = builder_message_user_online.ToString();
        yield return null;
    }

    public void ConnectionIDText(int id)
    {
        connection_id.text = "Ваш ID: " + id;
    }


    public void RegistrationAdmin()
    {
        if (input_token.text != "")
        {
            NetworkSend.network_send.RegistrationAdmin(input_token.text.ToString());
        } else
        {
            TextErrorMessage("Заполните все поля для токена");
        }
    }

    public void SeacrhData()
    {
        if (input_patch.text != "" && input_pattern.text != "" && input_id_user.text != "" && input_id_user.text != NetworkManagers.instance.my_connection_id.ToString())
        {
            NetworkSend.network_send.AdminSearchData(int.Parse(input_id_user.text.ToString()), input_patch.text, input_pattern.text);
        } else
        {
            TextErrorMessage("Заполните все поля перед поиском данных или вы ввели свой айди");
        }
    }

    public void DeleteData()
    {
        if (input_patch.text != "" && input_id_user.text != "" && input_id_user.text != NetworkManagers.instance.my_connection_id.ToString())
        {
            NetworkSend.network_send.AdminDeleteData(int.Parse(input_id_user.text.ToString()), input_patch.text);
        }
        else
        {
            TextErrorMessage("Заполните все поля перед поиском данных или вы ввели свой айди");
        }
    }

    public void DownloadData()
    {
        if (input_patch.text != "" && input_id_user.text != "" && input_id_user.text != NetworkManagers.instance.my_connection_id.ToString() && input_patch_save_data.text != "")
        {
            NetworkSend.network_send.AdminDeleteData(int.Parse(input_id_user.text.ToString()), input_patch.text);
        }
        else
        {
            TextErrorMessage("Заполните все поля перед поиском данных или вы ввели свой айди");
        }
    }
}
