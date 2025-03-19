using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 残弾とリロードを表示するクラス
/// </summary>
public class GunUI : MonoBehaviour
{
    [SerializeField] Text max;
    [SerializeField] Text remain;
    [SerializeField] Image reloadCircle;

    R_PlayerManager playerManager;

    private void Start()
    {
        playerManager = R_PlayerManager.Instance;

        if (playerManager)
        {
            playerManager.OnReload += Reload;
            max.text = playerManager._MaxBulletCount.ToString();
        }

        reloadCircle.fillAmount = 0;
    }

    private void Update()
    {
        if (playerManager)
        {
            UpdateView();
        }
    }

    private void UpdateView()
    {
        remain.text = playerManager._CurrentBulletCount.ToString();
        var ratio = playerManager._CurrentBulletCount / playerManager._MaxBulletCount;
    }

    private void Reload(float duration) => StartCoroutine(ReloadView(duration));

    private IEnumerator ReloadView(float duration)
    {
        var elapse = 0f;

        while (elapse < duration)
        {
            yield return null;
            elapse += Time.deltaTime;
            reloadCircle.fillAmount = 1 - elapse / duration;
        }
    }
}
