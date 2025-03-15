using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class y_Enemy : MonoBehaviour, IEnemy
{
    [SerializeField, Header("最大体力値")]
    float maxHP = 10.0f;
    [SerializeField, Header("攻撃力")]
    float attack = 1.0f;
    [SerializeField, Header("移動速度")]
    float moveSpeed = 1.0f;
    [SerializeField, Header("探索状態の視野距離")]
    float searchDistance = 5.0f;
    [SerializeField, Header("戦闘状態の視野距離")]
    float battleDistance = 8.0f;
    [SerializeField, Header("状態移行するときの秒数")]
    float modeTranstionTime = 1.0f;
    [SerializeField, Header("戦闘状態遷移時にターゲットの方向を向くのにかかる時間")]
    float targetFollowTime = 1.0f;
    [SerializeField, Header("1秒間で発射する数量")]
    int fireRate = 3;
    [SerializeField, Header("移動する場所を入れる")]
    Transform[] patrolPos;

    int currentPatrolIndex = 0; // 現在の巡回番号
    float currentHP = 0.0f; // 現在の体力
    float stopDistance = 0.1f; // 停止する距離
    bool isDead = false;    // 死んだかどうか
    NavMeshAgent agent; // ナビメッシュ
    Vector3 targetPosition; // 移動する位置
    Mode stateMode = Mode.SearchState; // 状態格納

    enum Mode // 状態管理
    {
        SearchState, // 探索状態
        BattleState, // 戦闘状態
    }

    // 初めの1回のみ処理
    void Start()
    {
        // 初期化
        stateMode = Mode.SearchState;
        currentHP = maxHP;
        agent = GetComponent<NavMeshAgent>(); // ナビメッシュ取得
        agent.speed = moveSpeed; // 移動速度の設定
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        if(isDead) return; // 死んでいるなら処理しない

        switch (stateMode)
        {
            case Mode.SearchState: // 探索状態時

                // パトロールするポイントが無ければ処理しない
                if (patrolPos.Length == 0) return;

                // 目的地に到達したら次のポイントに移動
                if (!agent.pathPending && agent.remainingDistance <= stopDistance)
                {
                    MoveToNextPoint(); // 次のポイントを設定する処理
                }
                break;
            case Mode.BattleState: // 戦闘状態時
                break;
        }
    }

    // 0.02秒毎に呼ばれる
    void FixedUpdate()
    {
        if (isDead) return; // 死んでいるなら処理しない

    }

    void MoveToNextPoint()
    {
        if (patrolPos.Length == 0) return;

        // 次のポイントを設定
        Vector3 targetPosition = new Vector3(
            patrolPos[currentPatrolIndex].position.x,
            transform.position.y,  // Y軸は固定
            patrolPos[currentPatrolIndex].position.z);

        agent.SetDestination(targetPosition); // 目的地設定
        agent.isStopped = false; // 移動再開

        // 次の目的地へ
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPos.Length; // ループする
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0.0f)
        {
            currentHP = 0.0f; // 0以下にしない
            isDead = true;
        }

        Debug.Log("TakeDamageが呼ばれた!!");
    }
}
