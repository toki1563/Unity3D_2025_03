using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームオーバー・ステージクリア通知のテスト
/// </summary>
public class E_GameStateReceiverTest : MonoBehaviour, IGameStateReceiver
{
    public Action OnGameStart => GameStart;

    public Action OnGameOver => GameOver;

    public Action OnStageClear => StageClear;

    public async void GameOver()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} GameOver (Delay 1s)");
    }

    public async void GameStart()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} GameStart (Delay 1s)");
    }

    public async void StageClear()
    {
        await Task.Delay(1000);
        Debug.Log($"{name} StageClear (Delay 1s)");
    }
}
