using UnityEngine;
using UnityEngine.UI;

public class NetworkManagers : MonoBehaviour
{
    public int my_connection_id;

    public int delete_player;

    public static NetworkManagers instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // �� ������� ���� ������� ������, ����� �� ������������� ����� �������.
        // ���� ����� ��������� ������� ������, ���� ���� �� �������� �����
        DontDestroyOnLoad(this);
        // ������������� ������� ������
        NetworkConfig.InitNetwork();
        NetworkConfig.ConnectToServer();
    }

    // ���� ����� ����������� ����� ����� ������� �� ����
    private void OnApplicationQuit()
    {
        NetworkConfig.DisconnectFromServer();
    }
}
