using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゴール判定
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField] GameStateController controller;
    [SerializeField] UnityEvent OnGoalAnimation;

    private void Reset()
    {
        if (FindAnyObjectByType<GameStateController>() is GameStateController c)
        {
            controller = c;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnGoalAnimation.Invoke();
        controller.StageClear();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGoalAnimation.Invoke();
        controller.StageClear();
    }
}
