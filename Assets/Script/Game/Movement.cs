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

    // ������ Plane�� ����ĳ���� �ϱ� ���� ����
    Plane plane;                // ������ �������� ������ �ٴ��� �����ϱ� ���� (�浹�� �����̶� �÷��̾��� ������ ����)
    Ray ray;                    // �ٴڿ� �浹�� ������ ����
    Vector3 hitPoint;           // ������ �ε����� ������ ����ϱ� ����

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

        // �ڽ��� �ɸ����� ��� �ó׸ӽ� ī�޶� ����
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        // ������ �ٴ��� �������� ���ΰ��� ��ġ ����
        plane = new Plane(transform.up, transform.position);
        GameTr = GameObject.Find("Game").transform;
        transform.parent = GameTr;
    }

    void Update()
    {
        // �ڽ��� �ɸ���(��Ʈ��ũ ��ü)�� ��Ʈ��
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

        float forward = Vector3.Dot(moveDir, transform.forward);                     // ������ ����, ���� ������ ���� ����
        float strafe = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        // ������ �ٴڿ� ray �߻��Ͽ� �浹 ������ �Ÿ��� enter ������ ����
        plane.Raycast(ray, out enter);
        // ������ �ٴڿ� ���̰� �浹�� ��ǩ�� ����
        hitPoint = ray.GetPoint(enter);

        // ȸ���ؾ��� ������ ���� ���
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
            {                                       //�Ϲ��Ѿ�
                BulletTimeSave = Time.time;
                BulletShootDelay = true;
                Instantiate(BulletPrefab, GunTr.position, Quaternion.Euler(transform.eulerAngles), GameTr);
            }

            if (Time.time > BulletTimeSave + Delay)
            {                                //����ӵ�
                BulletShootDelay = false;
            }
        }
    }

    public void DisconnectedPhoton() => PhotonNetwork.Disconnect();
}
