using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    static GameMng _instance;

    public static GameMng I
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
    }

    public List<Movement> players = new List<Movement>();
    public int PlayerCnt = 0;

    [Space(20)]
    [Header("[ Death ]")]
    public GameObject DeathScene;
    [SerializeField]
    TMPro.TMP_Text LivePlayerText;

    public void GameEnd()
    {
        foreach (var player in players)
        {
            if (player.HP > 0)
                LivePlayerText.text += player.PlayerName + '\n';
        }
    }

    public IEnumerator RenewalPlayer()
    {
        yield return new WaitForSeconds(1.0f);
        players.Clear();
        Movement[] users = GameObject.Find("Players").GetComponentsInChildren<Movement>();
        foreach (Movement user in users)
        {
            players.Add(user);
        }
    }
}
