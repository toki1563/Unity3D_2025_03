using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 各シーンを管理するクラス
/// </summary>
[CreateAssetMenu()]
public class GameSceneManager : ScriptableObject
{
    [SerializeField] int stageCount = 2;

    [SerializeField] string stageScenePrefix = "Stage";
    [SerializeField] string titleSceneName = "Title";
    [SerializeField] string clearSceneName = "Clear";

    public void LoadTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void LoadStage(int stageNumber)
    {

        if (stageNumber > 0 && stageNumber <= stageCount)
        {
            SceneManager.LoadScene(stageScenePrefix + stageNumber);
        }
        else
        {
            Debug.Log($"Stage{stageNumber}は存在しません");
        }
    }

    public void NextStage()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        var name = currentSceneName[..stageScenePrefix.Length];
        var number = currentSceneName[stageScenePrefix.Length..];

        if (name == stageScenePrefix)
        {
            var stage = int.Parse(number) + 1;

            if (stage <= stageCount)
            {
                LoadStage(stage);
            }
            else
            {
                LoadClear();
            }
        }
        else
        {
            LoadStage(1);
        }
    }

    public void LoadClear()
    {
        SceneManager.LoadScene(clearSceneName);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
