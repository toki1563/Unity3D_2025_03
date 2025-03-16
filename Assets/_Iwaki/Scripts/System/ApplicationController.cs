using UnityEngine;

/// <summary>
/// アプリケーションの操作
/// </summary>
public class ApplicationController : MonoBehaviour
{
    public static void CloseApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
