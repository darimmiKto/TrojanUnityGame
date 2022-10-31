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
        // Не удалять этот игровой объект, когда мы переключаемся между сценами.
        // Этот метод сохраняет игровой объект, даже если мы поменяли сцену
        DontDestroyOnLoad(this);
        // Иницилизируем сетевые классы
        NetworkConfig.InitNetwork();
        NetworkConfig.ConnectToServer();
    }

    // Этот метод выполняется когда игрок выходит из игры
    private void OnApplicationQuit()
    {
        NetworkConfig.DisconnectFromServer();
    }
}
