using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 制限時間を表示する
/// </summary>
public class TimerUI : MonoBehaviour
{
    [SerializeField] string timerFormat = "mm':'ss'.'ff";
    [SerializeField] Text text;

    GameTimer gameTimer;

    private void Start()
    {
        if (gameTimer == null)
        {
            gameTimer = FindAnyObjectByType<GameTimer>();
        }
    }

    private void Update()
    {
        text.text = gameTimer.GetTimerText(timerFormat);
    }
}
