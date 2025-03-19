using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージ内の制限時間の管理、タイムアップの通知を行うクラス。
/// </summary>
public class GameTimer : MonoBehaviour, IGameOverSender, IGameStateReceiver
{
    [SerializeField] bool enableTimer;
    [SerializeField, Header("制限時間(秒)")] float timeLimit = 180;

    float currentTimer;

    public event Action SendGameOver;

    public Action OnGameStart => TimerStart;
    public Action OnGameOver => TimerStop;
    public Action OnStageClear => TimerStop;


    private void Start()
    {
        currentTimer = timeLimit;
    }

    private void Update()
    {
        if (enableTimer)
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0)
            {
                currentTimer = 0;
                TimerStop();
                SendGameOver.Invoke();
            }
        }
    }

    public string GetTimerText(string format)
    {
        var timeSpan = TimeSpan.FromSeconds(currentTimer);
        var formattedTime = timeSpan.ToString(format);
        return formattedTime;
    }

    public void TimerStart()
    {
        enableTimer = true;
    }

    public void TimerStop()
    {
        enableTimer = false;
    }
}
