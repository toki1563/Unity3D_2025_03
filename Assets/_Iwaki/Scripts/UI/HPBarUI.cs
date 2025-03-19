using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー体力UI
/// </summary>
public class HPBarUI : MonoBehaviour
{
    [SerializeField] Image health;
    [SerializeField] Image damage;
    [SerializeField] float timeToStartDecreaseDamageBar = 1;
    [SerializeField] float damageBarDecreaseDuration = 1;
 
    R_PlayerManager playerManager;
    float prevHP;
    float prevDamagedTime;
    float targetRatio = 1;

    private void Start()
    {
        playerManager = R_PlayerManager.Instance;
        if (playerManager) prevHP = playerManager._MaxHP;

        health.fillAmount = 1;
        damage.fillAmount = 1;
    }

    private void Update()
    {
        if (playerManager && playerManager._CurrentHP != prevHP)
        {
            prevDamagedTime = Time.time;
            prevHP = playerManager._CurrentHP;
            UpdateView();
        }

        if (prevDamagedTime + timeToStartDecreaseDamageBar < Time.time)
        {
            damage.fillAmount = Mathf.MoveTowards(damage.fillAmount, targetRatio, damageBarDecreaseDuration * Time.deltaTime);
        }
    }

    private void UpdateView()
    {
        targetRatio = playerManager._CurrentHP / playerManager._MaxHP;
        health.fillAmount = targetRatio;
    }
}
