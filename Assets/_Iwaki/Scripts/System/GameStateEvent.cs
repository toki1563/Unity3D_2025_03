using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// コンポーネントからゲームステート遷移時イベントを登録するクラス
/// </summary>
public class GameStateEvent : MonoBehaviour, IGameStateReceiver
{
    [SerializeField] UnityEvent onGameStart;
    [SerializeField] UnityEvent onGameOver;
    [SerializeField] UnityEvent onStageClear;

    public Action OnGameStart => onGameStart.Invoke;

    public Action OnGameOver => onGameOver.Invoke;

    public Action OnStageClear => onStageClear.Invoke;
}
