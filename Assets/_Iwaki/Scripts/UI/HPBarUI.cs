using UnityEngine;

/// <summary>
/// プレイヤー体力UI
/// </summary>
public class HPBarUI : MonoBehaviour
{
    [SerializeField] R_PlayerManager playerManager;

    private void Reset()
    {
        if (FindAnyObjectByType<R_PlayerManager>() is R_PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }
    }

    private void Update()
    {
        var ratio = playerManager._CurrentHP / playerManager._MaxHP;

    }
}
