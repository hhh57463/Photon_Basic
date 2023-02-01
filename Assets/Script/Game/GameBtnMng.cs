using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBtnMng : MonoBehaviour
{
    public void GoMenuBtn()
    {
        Photon.Pun.PhotonNetwork.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    Cinemachine.CinemachineVirtualCamera virtualCamera;
    public void WatchPlayer()
    {
        GameMng.I.DeathScene.SetActive(false);
        virtualCamera = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        foreach(var player in GameMng.I.players)
        {
            if(player.HP > 0)
            {
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
                break;
            }
        }
    }
}
