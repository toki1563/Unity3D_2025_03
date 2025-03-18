using UnityEngine;

public class R_Bullet : MonoBehaviour
{
    [SerializeField,Header("弾の攻撃力")]
    int damage = 1;

    // 敵に当たった時
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IDamage enemy = other.GetComponent<IDamage>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
