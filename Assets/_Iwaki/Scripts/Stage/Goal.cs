using UnityEngine;

/// <summary>
/// ゴール判定
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField] GameStateController controller;

    private void OnCollisionEnter(Collision collision)
    {
        controller.StageClear();
    }

    private void OnTriggerEnter(Collider other)
    {
        controller.StageClear();
    }
}
