using Cinemachine;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using UnityEngine;

/// <summary>
/// ゲームステートに合わせたカメラ遷移
/// </summary>
public class CameraController : MonoBehaviour, IGameStateReceiver
{
    [SerializeField] CinemachineVirtualCameraBase stageInCam;
    [SerializeField] CinemachineVirtualCameraBase playingCam;
    [SerializeField] CinemachineVirtualCameraBase gameOverCam;
    [SerializeField] CinemachineVirtualCameraBase stageClearCam;

    public Action OnGameStart => () => playingCam.MoveToTopOfPrioritySubqueue();
    public Action OnGameOver => () => gameOverCam.MoveToTopOfPrioritySubqueue();
    public Action OnStageClear => () => stageClearCam.MoveToTopOfPrioritySubqueue();

    private void Start()
    {
        var cams = new[]{ stageInCam, playingCam, gameOverCam, stageClearCam };

        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].gameObject.SetActive(true);
            cams[i].enabled = true;
            cams[i].Priority = 1;
        }

        stageInCam.MoveToTopOfPrioritySubqueue();
    }

    public void SetTarget(Transform transform)
    {
        var cams = GetTarget();

        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].LookAt = transform;
            cams[i].Follow = transform;
        }

        Debug.Log($"SetTarget {transform.name}.transform");
    }

    public CinemachineVirtualCameraBase[] GetTarget()
    {
        return new[] { stageInCam, playingCam, gameOverCam, stageClearCam };
    }
}