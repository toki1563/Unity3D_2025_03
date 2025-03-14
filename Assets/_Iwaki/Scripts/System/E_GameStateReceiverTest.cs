using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームオーバー・ステージクリア通知のテスト
/// </summary>
public class E_GameStateReceiverTest : MonoBehaviour, IGameOverObserver, IStageClearObserver, IGameStartObserver
{
    public async void OnGameOver()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} GameOver");
    }

    public async void OnGameStart()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} GameStart");
    }

    public async void OnStageClear()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} StageClear");
    }
}
