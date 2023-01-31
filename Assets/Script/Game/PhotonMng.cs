using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonMng : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                            // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.GameVersion = Mng.I.version;                              // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.NickName = Mng.I.NickName;                                // 유저 아이디 할당
        Debug.Log(PhotonNetwork.SendRate);                                      // 포톤 서버와 통신 횟수 설정 (초당 30회)
        PhotonNetwork.ConnectUsingSettings();                                   // 서버 접속
    }

    /**
     *@brief 포톤 서버에 접속 후 호출되는 콜백 함수
     */
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();                                              // 로비 입장
    }

    /**
     *@brief 로비에 접속 후 호출되는 콜백 함수
     */
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();                                         // 랜덤 매치메이킹 기능
    }

    /**
     *@brief 랜덤 룸 입장 실패했을 때 호출되는 콜백 함수
     *@param returnCode 코드번호로 어떤 에러가 떴는지
     *@param message 에러 문자열
     *@var ro 룸 속성 정의 클래스
     */
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");

        RoomOptions ro = new RoomOptions();                                     // 룸 속성 정의
        ro.MaxPlayers = 20;                                                     // 최대 접속자 수 (무료는 20명까지 가능)
        ro.IsOpen = true;                                                       // 룸 오픈 여부
        ro.IsVisible = true;                                                    // 로비에서 룸 목록에 노출 시킬지 여부

        PhotonNetwork.CreateRoom("My Room", ro);                                // My Room이라는 ro의  속성을 가진 룸 생성
    }

    /**
     *@brief 룸 생성이 완료도니 후 호출되는 콜백 함수
     */
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");             // 현재 룸 이름 출력
    }

    /**
     *@brief 룸에 입장한 후 호출되는 콜백 함수
     */
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 룸에 접속한 사용자 정보 확인
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");          // 플레이어 이름, 플레이어 고유 번호 출력
        }

        // 캐릭터 출현 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(0, points.Length);

        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);          // 캐릭터 생성
    }
}
