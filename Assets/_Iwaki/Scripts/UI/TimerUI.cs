using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 制限時間を表示する
/// </summary>
public class TimerUI : MonoBehaviour
{
    [SerializeField] GameTimer gameTimer;
    [SerializeField] string timerFormat = "mm':'ss'.'ff";
    [SerializeField] Text text;

    private void Update()
    {
        text.text = gameTimer.GetTimerText(timerFormat);
    }
}
