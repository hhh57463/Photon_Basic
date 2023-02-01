using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonMng : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                            // ���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.GameVersion = Mng.I.version;                              // ���� ������ �������� ���� ���
        PhotonNetwork.NickName = Mng.I.NickName;                                // ���� ���̵� �Ҵ�
        Debug.Log(PhotonNetwork.SendRate);                                      // ���� ������ ��� Ƚ�� ���� (�ʴ� 30ȸ)
        PhotonNetwork.ConnectUsingSettings();                                   // ���� ����
    }

    /**
     *@brief ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
     */
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();                                              // �κ� ����
    }

    /**
     *@brief �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
     */
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        if (Mng.I.MakingRoom)
        {
            PhotonNetwork.CreateRoom(Mng.I.RoomName);                                   // �ش� �̸����� �� ����
        }
        else
        { 
            if (Mng.I.RandomRoom)
                PhotonNetwork.JoinRandomRoom();                                         // ���� ��ġ����ŷ ���
            else
                PhotonNetwork.JoinRoom(Mng.I.RoomName);                                 // �ش� �̸��� ���� �� ����
        }
    }

    /**
     *@brief ���� �� ���� �������� �� ȣ��Ǵ� �ݹ� �Լ�
     *@param returnCode �ڵ��ȣ�� � ������ ������
     *@param message ���� ���ڿ�
     *@var ro �� �Ӽ� ���� Ŭ����
     */
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");

        RoomOptions ro = new RoomOptions();                                     // �� �Ӽ� ����
        ro.MaxPlayers = 20;                                                     // �ִ� ������ �� (����� 20����� ����)
        ro.IsOpen = true;                                                       // �� ���� ����
        ro.IsVisible = true;                                                    // �κ񿡼� �� ��Ͽ� ���� ��ų�� ����

        PhotonNetwork.CreateRoom("My Room", ro);                                // My Room�̶�� ro��  �Ӽ��� ���� �� ����
    }

    /**
     *@brief �� ������ �Ϸᵵ�� �� ȣ��Ǵ� �ݹ� �Լ�
     */
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");             // ���� �� �̸� ���
    }

    /**
     *@brief �뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
     */
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // �뿡 ������ ����� ���� Ȯ��
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");          // �÷��̾� �̸�, �÷��̾� ���� ��ȣ ���
        }

        // ĳ���� ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(0, points.Length);

        Movement Player = PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0).GetComponent<Movement>();          // ĳ���� ����
        Player.PlayerName = Mng.I.NickName;
        GameMng.I.players.Add(Player);
        GameMng.I.PlayerCnt++;
    }

    /**
    *@brief ���� �뿡 ���ο� ������ �������� ȣ��Ǵ� �ݹ� �Լ�
    */
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���� ����");
        GameMng.I.PlayerCnt++;
        StartCoroutine(GameMng.I.RenewalPlayer());
    }

    /**
    *@brief ���� �뿡 ������ �������� ȣ��Ǵ� �ݹ� �Լ�
    */
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("���� ����");
        GameMng.I.PlayerCnt--;
        StartCoroutine(GameMng.I.RenewalPlayer());
    }

    /**
    *@brief ������ �������� ȣ��Ǵ� �ݹ� �Լ�
    */
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"{cause}");
    }
}
