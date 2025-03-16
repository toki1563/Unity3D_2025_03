using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField, Header("探索の視野角(°)")]
    float searchOfView = 45.0f;
    [SerializeField, Header("戦闘状態の視野距離")]
    float battleDistance = 8.0f;
    [SerializeField, Header("戦闘の視野角(°)")]
    float battleOfView = 90.0f;
    [SerializeField, Header("状態移行するときの秒数")]
    float modeTranstionTime = 1.0f;
    [SerializeField, Header("戦闘状態遷移時にターゲットの方向を向く速度")]
    float targetFollowSpeed = 120.0f;
    [SerializeField, Header("1秒間で発射する数量")]
    int fireRate = 3;
    [SerializeField, Header("プレイヤーを格納")]
    Transform player;
    [SerializeField, Header("移動する場所を入れる")]
    Transform[] patrolPos;
    [SerializeField, Header("無視する用の壁のレイヤーを入れる")]
    LayerMask obstacleMask;
    [SerializeField, Header("デバッグ用(探索判定の可視化)")]
    bool isSearchDebug;
    [SerializeField, Header("デバッグ用(戦闘判定の可視化)")]
    bool isButtleDebug;

    int currentPatrolIndex = 0; // 現在の巡回番号
    float currentHP = 0.0f; // 現在の体力
    float stopDistance = 0.1f; // 停止する距離
    float angleToPlayer = 0.0f; // プレイヤー向きの格納
    bool isDead = false;    // 死んだかどうか
    bool isChangingState = false; // 状態遷移中かどうか
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
        agent.angularSpeed = targetFollowSpeed; // プレイヤを見る速度
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


                // プレイヤーを索敵
                if (CanSearchPlayer() && !isChangingState)
                {
                    agent.isStopped = true; // 移動停止

                    // バトルモード
                    StartCoroutine(ChangeStateDelay(Mode.BattleState));
                    Debug.Log("プレイヤー発見");
                }
                break;
            case Mode.BattleState: // 戦闘状態時

                // 攻撃処理を書く

                // プレイヤーが索敵外になったら
                if (!CanSearchPlayer() && !isChangingState)
                {
                    agent.isStopped = false; // 移動再開
                    // バトルモード
                    StartCoroutine(ChangeStateDelay(Mode.SearchState));
                    Debug.Log("プレイヤー発見");
                }
                break;
        }
    }

    // 0.02秒毎に呼ばれる
    void FixedUpdate()
    {
        if (isDead) return; // 死んでいるなら処理しない

    }

    // 状態遷移を一定時間後に行う
    IEnumerator ChangeStateDelay(Mode newMode)
    {
        isChangingState = true;
        yield return new WaitForSeconds(modeTranstionTime);
        Debug.Log("バトルモード");
        stateMode = newMode;
        isChangingState = false;
    }

    // 次の移動地点の処理
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

    // プレイヤーの判定
    bool CanSearchPlayer()
    {
        if (player == null) return false;

        // プレイヤーとの距離の計算
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        switch(stateMode)
        {
            case Mode.SearchState:
                // 距離の確認
                if (distanceToPlayer > searchDistance) return false;

                // 視野角の確認
                directionToPlayer.y = 0; // Y軸を無視する
                angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer); // プレイヤーの向き
                if (angleToPlayer > searchOfView / 2) return false;
                break;
            case Mode.BattleState:

                if (distanceToPlayer > battleDistance) return false;

                directionToPlayer.y = 0; // Y軸を無視する
                angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                if (angleToPlayer > battleOfView / 2) return false;
                break;
        }

        // ③ レイキャストで障害物チェック
        if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
        {
            return false; // 壁がある時は感知しない
        }

        return true;
    }

    // ダメージを受けた時の処理
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

    // デバッグ可視化用
    void OnDrawGizmos()
    {
        if (isSearchDebug)
        {
            // 索敵範囲
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchDistance);

            // 視野角の表示
            Vector3 leftBoundary = Quaternion.Euler(0, -searchOfView / 2, 0) * transform.forward * searchDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, searchOfView / 2, 0) * transform.forward * searchDistance;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }

        if (isButtleDebug)
        {
            // 索敵範囲
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, battleDistance);

            // 視野角の表示
            Vector3 leftBoundary = Quaternion.Euler(0, -battleOfView / 2, 0) * transform.forward * battleDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, battleOfView / 2, 0) * transform.forward * battleDistance;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }
    }
}
