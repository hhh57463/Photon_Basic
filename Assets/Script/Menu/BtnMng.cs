using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnMng : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_InputField NickNameField;
    public void RandomRoom()
    {
        if (NickNameField.text != "")
        {
            Mng.I.NickName = NickNameField.text;
            Mng.I.RandomRoom = true;
            Mng.I.MakingRoom = false;
            SceneManager.LoadScene("PhotonGame");
        }
        else
        {
            Debug.Log("닉네임 입력");
        }
    }

    [SerializeField]
    TMPro.TMP_InputField RoomNameField;
    public void MakeBtn()
    {
        Mng.I.RoomName = RoomNameField.text;
        Mng.I.RandomRoom = false;
        Mng.I.MakingRoom = true;
        SceneManager.LoadScene("PhotonGame");
    }

    public void JoinBtn()
    {
        Mng.I.RoomName = RoomNameField.text;
        Mng.I.RandomRoom = false;
        Mng.I.MakingRoom = false;
        SceneManager.LoadScene("PhotonGame");
    }
}
