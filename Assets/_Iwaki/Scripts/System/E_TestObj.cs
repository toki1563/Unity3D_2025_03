using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームオーバー・ステージクリア通知のテスト
/// </summary>
public class E_TestObj : MonoBehaviour, IGameOverObserver, IStageClearObserver
{
    public async void OnGameOver()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} GameOver");
    }

    public async void OnStageClear()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} StageClear");
    }
}
