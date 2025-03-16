using System.Collections;
using UnityEngine;

/// <summary>
/// シーンが読み込まれてから一定時間経過後にゲームを開始させるコンポーネント
/// </summary>
public class GameStarter : MonoBehaviour
{
    [SerializeField] GameStateController controller;
    [SerializeField] float timeToStart;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToStart);
        controller.GameStart();
    }
}
