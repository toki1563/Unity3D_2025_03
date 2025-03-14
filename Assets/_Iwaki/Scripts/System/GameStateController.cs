using UnityEngine;

/// <summary>
/// ゲームオーバー・ステージクリアを通知するクラス
/// </summary>
public class GameStateController : MonoBehaviour
{
    [SerializeField] private GameState currentState = GameState.WaitStart;

    private void Start()
    {
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            switch(obj)
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
    }

    public void GameStart()
    {
        if (currentState != GameState.WaitStart) return;

        currentState = GameState.Playing;

        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is IGameStartObserver observer)
            {
                observer.OnGameStart();
            }
        }
    }

    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.GameOver;

        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is IGameOverObserver observer)
            {
                observer.OnGameOver();
            }
        }
    }

    public void StageClear()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.StageClear;

        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is IStageClearObserver observer)
            {
                observer.OnStageClear();
            }
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
