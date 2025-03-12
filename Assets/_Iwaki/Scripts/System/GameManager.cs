using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲームスタート・ゲームオーバー・ステージクリアなどの管理（各システムのインターフェース）
/// </summary>
public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public UnityEvent OnStageClear;

    public void GameOver()
    {
        // ゲームオーバーを通知
        OnGameOver.Invoke();

        // ゲームオーバー演出完了待機

        // 現在のシーンをリロード
    }

    public void StageClear()
    {
        OnStageClear.Invoke();

        // 演出完了待機

        // シーン遷移処理
    }
}
