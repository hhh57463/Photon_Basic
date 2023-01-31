using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    new Transform transform;
    Animator animator;
    new Camera camera;

    // 가상의 Plane에 레이캐스팅 하기 위한 변수
    Plane plane;                // 지정한 지점에서 가상의 바닥을 생성하기 위함 (충돌한 지점이랑 플레이어의 연산을 위함)
    Ray ray;                    // 바닥에 충돌할 지점을 위해
    Vector3 hitPoint;           // 광선과 부딪히는 지점을 계산하기 위함

    public float moveSpeed = 10.0f;

    PhotonView pv;
    CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    GameObject BulletPrefab;

    Transform GameTr;

    [SerializeField]
    Transform GunTr;

    public int HP = 5;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        // 자신의 케릭터일 경우 시네머신 카메라 연결
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        // 가상의 바닥을 기준으로 주인공의 위치 생성
        plane = new Plane(transform.up, transform.position);
        GameTr = GameObject.Find("Game").transform;
        transform.parent = GameTr;
    }

    void Update()
    {
        // 자신의 케릭터(네트워크 객체)만 컨트롤
        if (pv.IsMine)
        {
            Move();
            Turn();
            Fire();
        }
        if(HP <= 0)
        {
            DisconnectedPhoton();
        }
    }

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0, moveDir.z);

        controller.SimpleMove(moveDir * moveSpeed);

        float forward = Vector3.Dot(moveDir, transform.forward);                     // 벡터의 내적, 벡터 사이의 각도 측정
        float strafe = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        // 가상의 바닥에 ray 발사하여 충돌 지점의 거리를 enter 변수에 저장
        plane.Raycast(ray, out enter);
        // 가상의 바닥에 레이가 충돌한 좌푯값 추출
        hitPoint = ray.GetPoint(enter);

        // 회전해야할 방향의 벡터 계산
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0;

        transform.localRotation = Quaternion.LookRotation(lookDir);
    }

    float BulletTimeSave;
    float Delay = 0.5f;
    bool BulletShootDelay;
    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if (!BulletShootDelay)
            {                                       //일반총알
                BulletTimeSave = Time.time;
                BulletShootDelay = true;
                Instantiate(BulletPrefab, GunTr.position, Quaternion.Euler(transform.eulerAngles), GameTr);
            }

            if (Time.time > BulletTimeSave + Delay)
            {                                //연사속도
                BulletShootDelay = false;
            }
        }
    }

    public void DisconnectedPhoton() => PhotonNetwork.Disconnect();
}
