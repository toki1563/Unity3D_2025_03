using UnityEngine;

/// <summary>
/// ゲームオーバー・ステージクリアを通知するクラス
/// </summary>
public class GameStateController : MonoBehaviour
{
    [SerializeField] private GameState currentState = GameState.Default;

    private void Start()
    {
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is IGameOverSender s1)
            {
                s1.SendGameOver += GameOver;
            }

            if (obj is IStageClearSender s2)
            {
                s2.SendStageClear += StageClear;
            }
        }
    }

    public void GameOver()
    {
        if (currentState != GameState.Default) return;

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
        if (currentState != GameState.Default) return;

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
    Default,
    GameOver,
    StageClear,
}
