using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームステートの変更を通知するクラス
/// </summary>
public class GameStateController : MonoBehaviour
{
    [SerializeField] private GameState currentState = GameState.WaitStart;

    private readonly List<IGameStateReceiver> receivers = new();

    private void Start()
    {
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            switch (obj)
            {
                case IGameStartSender s:
                    s.SendGameStart += GameStart;
                    break;
                case IGameOverSender s:
                    s.SendGameOver += GameOver;
                    break;
                case IStageClearSender s:
                    s.SendStageClear += StageClear;
                    break;
            }
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
        if (currentState != GameState.Playing) return;

        Debug.Log("GameOver");
        currentState = GameState.GameOver;

        for (int i = 0; i < receivers.Count; i++)
        {
            receivers[i].OnGameOver?.Invoke();
        }
    }

    public void StageClear()
    {
        if (currentState != GameState.Playing) return;

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
