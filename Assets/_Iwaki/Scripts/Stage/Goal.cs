using System.Collections.Generic;
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
        if (FindAnyObjectByType<GameStateController>() is GameStateController gameStateController)
        {
            controller = gameStateController;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == R_PlayerManager.Instance.transform)
        {
            OnGoalAnimation.Invoke();
            controller.StageClear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == R_PlayerManager.Instance.transform)
        {
            OnGoalAnimation.Invoke();
            controller.StageClear();
        }
    }
}
