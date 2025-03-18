using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ゲームステートの変更を通知するクラス
/// </summary>
public class GameStateController : MonoBehaviour
{
    [SerializeField] GameState currentState = GameState.WaitStart;

    private readonly List<IGameStateReceiver> receivers = new();

    private void Start()
    {
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is IGameStartSender startSender) startSender.SendGameStart += GameStart;
            if (obj is IGameOverSender gameOverSender) gameOverSender.SendGameOver += GameOver;
            if (obj is IStageClearSender clearSender) clearSender.SendStageClear += StageClear;
        }

        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is IGameStateReceiver receiver)
            {
                receivers.Add(receiver);
            }
        }
    }

    public void GameStart()
    {
        if (currentState != GameState.WaitStart) return;

        Debug.Log("GameStart");
        currentState = GameState.Playing;

        for (int i = 0; i < receivers.Count; i++)
        {
            receivers[i].OnGameStart?.Invoke();
        }
    }

    public void GameOver()
    {
        if (currentState == GameState.GameOver) return;

        if (currentState == GameState.WaitStart) print("ゲーム開始より前にゲームオーバーが呼び出されました");

        Debug.Log("GameOver");
        currentState = GameState.GameOver;

        for (int i = 0; i < receivers.Count; i++)
        {
            receivers[i].OnGameOver?.Invoke();
        }
    }

    public void StageClear()
    {
        if (currentState == GameState.GameOver) return;

        if (currentState == GameState.WaitStart) print("ゲーム開始より前にステージクリアが呼び出されました");

        Debug.Log("StageClear");
        currentState = GameState.StageClear;

        for (int i = 0; i < receivers.Count; i++)
        {
            receivers[i].OnStageClear?.Invoke();
        }
    }
}

public enum GameState
{
    WaitStart,
    Playing,
    GameOver,
    StageClear,
}
