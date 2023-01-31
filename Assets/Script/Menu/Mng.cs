using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mng : MonoBehaviour
{
    public static Mng _instance;

    public static Mng I
    {
        get
        {
            if (_instance.Equals(null))
                Debug.Log("instance is null");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(1280, 720, false);
    }

    public readonly string version = "1.0f";
    public string NickName;
}
