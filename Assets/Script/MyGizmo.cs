using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float _radius = 0.3f;

    void OnDrawGizmos()
    {
        Gizmos.color = _color;                                      // 기즈모 색상 설정
        Gizmos.DrawSphere(transform.position, _radius);             // 구 형태의 기즈모 생성
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
