using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_Bullet : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    float bulletSpeed = 5.0f;
    [SerializeField, Header("弾の消滅する時間")]
    float lifeTime = 8.0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed; // 前方向に発射
        Destroy(gameObject, lifeTime); // 数秒後に削除
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // ダメージを送る
            IDamage player = other.GetComponent<IDamage>();
            player.TakeDamage(1);
            Destroy(gameObject); // ヒット時に削除
        }
    }
}
