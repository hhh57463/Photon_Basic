using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginBtn : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_InputField NickNameField;
    public void Login()
    {
        if (NickNameField.text != "")
        {
            Mng.I.NickName = NickNameField.text;
            SceneManager.LoadScene("PhotonGame");
        }
        else
        {
            Debug.Log("닉네임 입력");
        }
    }
}
