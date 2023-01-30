using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        // ������ �ٴ��� �������� ���ΰ��� ��ġ ����
        plane = new Plane(transform.up, transform.position);
    }

    void Update()
    {
        Move();
        Turn();
    }

    void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);

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
}
