using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー体力UI
/// </summary>
public class HPBarUI : MonoBehaviour
{
    [SerializeField] Slider health;
    [SerializeField] Slider damage;
    [SerializeField] float timeToStartDecreaseDamageBar = 1;
    [SerializeField] float damageBarDecreaseDuration = 1;
 
    R_PlayerManager playerManager;
    float prevHP;
    float prevDamagedTime;
    float targetRatio = 1;

    private void Start()
    {
        playerManager = R_PlayerManager.Instance;
        prevHP = playerManager._MaxHP;

        health.value = 1;
        damage.value = 1;
    }

    private void Update()
    {
        if (playerManager._CurrentHP != prevHP)
        {
            prevDamagedTime = Time.time;
            prevHP = playerManager._CurrentHP;
            UpdateView();
        }

        if (prevDamagedTime + timeToStartDecreaseDamageBar < Time.time)
        {
            damage.value = Mathf.MoveTowards(damage.value, targetRatio, damageBarDecreaseDuration * Time.deltaTime);
        }
    }

    private void UpdateView()
    {
        targetRatio = playerManager._CurrentHP / playerManager._MaxHP;
        health.value = targetRatio;
    }
}
