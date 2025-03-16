using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_Player : MonoBehaviour
{
    float moveSpeed = 5.0f; // 移動速度
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 移動処理
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
    }

    // 当たった時
    private void OnTriggerEnter(Collider other)
    {
        // インターフェースを呼び出すテスト
        if(other.gameObject.CompareTag("Enemy"))
        {
            IDamage enemy = other.GetComponent<IDamage>();
            enemy.TakeDamage(5);

            Debug.Log("aaa");
        }
    }
}
