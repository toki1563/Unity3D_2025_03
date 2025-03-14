using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// シーンのロード処理（アニメーションから呼び出す用）
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameSceneManager gameSceneManager;
    [SerializeField] float transitionTime = 1;

    private async Task WaitTransition()
    {
        await Task.Delay((int)(transitionTime * 1000));
    }

    public async void Title()
    {
        await WaitTransition();
        gameSceneManager.LoadTitle();
    }

    public async void Stage(int stage)
    {
        await WaitTransition();
        gameSceneManager.LoadStage(stage);
    }

    public async void NextStage()
    {
        await WaitTransition();
        gameSceneManager.NextStage();
    }

    public async void Clear()
    {
        await WaitTransition();
        gameSceneManager.LoadClear();
    }

    public async void GameOver()
    {
        await WaitTransition();
        gameSceneManager.GameOver();
    }
}
