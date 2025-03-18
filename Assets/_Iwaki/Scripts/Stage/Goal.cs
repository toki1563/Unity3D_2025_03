using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゴール判定
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField] GameStateController controller;
    [SerializeField] List<Transform> canGoalObjects;
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
        if (canGoalObjects.Contains(collision.transform))
        {
            OnGoalAnimation.Invoke();
            controller.StageClear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canGoalObjects.Contains(other.transform))
        {
            OnGoalAnimation.Invoke();
            controller.StageClear();
        }
    }
}
