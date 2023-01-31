using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 30.0f;
    void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Movement enemy = col.GetComponent<Movement>();
            enemy.HP--;

            Destroy(gameObject);
        }
    }
}
