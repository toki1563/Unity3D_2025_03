
using System;

/// <summary>
/// ゲームオーバー時に通知を受け取る
/// </summary>
interface IGameOverObserver
{
    void OnGameOver();
}

/// <summary>
/// ステージクリア時に通知を受け取る
/// </summary>
interface IStageClearObserver
{
    void OnStageClear();
}

interface IGameStartObserver
{
    void OnGameStart();
}

/// <summary>
/// Start時にGameStateControllerの関数が自動的に登録されるアクションを実装します
/// </summary>
interface IGameOverSender
{
    /// <summary>
    /// ゲームオーバーを通知します
    /// </summary>
    event Action SendGameOver;
}

/// <summary>
/// Start時にGameStateControllerの関数が自動的に登録されるアクションを実装します
/// </summary>
interface IStageClearSender
{
    /// <summary>
    /// スレージクリアを通知します
    /// </summary>
    event Action SendStageClear;
}

interface IGameStartSender
{
    event Action SendGameStart;
}