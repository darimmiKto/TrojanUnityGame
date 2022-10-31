using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers : MonoBehaviour
{
    // ������ ������� ������
    [SerializeField] public Dictionary<int, GameObject> player_list = new Dictionary<int, GameObject>();

    public static GameManagers instance;

    private void Awake()
    {
        instance = this;
    }
}
