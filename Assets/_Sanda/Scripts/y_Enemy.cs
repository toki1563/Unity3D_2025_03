using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class y_Enemy : MonoBehaviour, IDamage
{
    [SerializeField, Header("最大体力値")]
    float maxHP = 2.0f;
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
    [SerializeField, Header("探索状態に移行するときの秒数")]
    float searchModeTransTime = 0.5f;
    [SerializeField, Header("戦闘状態に移行するときの秒数")]
    float battleModeTransTime = 1.0f;
    [SerializeField, Header("戦闘状態遷移時にターゲットの方向を向く速度")]
    float targetFollowSpeed = 2.0f;
    [SerializeField, Header("敵の死亡までの時間")]
    float enemyDestroyTime = 1.5f;
    [SerializeField, Header("各ポイントでの待機時間")]
    float[] waitTimes = {};
    [SerializeField, Header("1秒間で発射する数量")]
    int fireRate = 3;
    [SerializeField, Header("弾のプレハブ")]
    GameObject bulletPrefab;
    [SerializeField, Header("プレイヤーを格納")]
    Transform player;
    [SerializeField, Header("弾の発射位置を格納")]
    Transform bullet;
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
    Coroutine fireCoroutine; // 射撃用コルーチン
    Coroutine waitCoroutine; // 停止時のコルーチン
    Vector3 targetPosition; // 移動する位置
    Mode stateMode = Mode.SearchState; // 状態格納
    Animator animator; // アニメーター


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
        animator = GetComponent<Animator>();
        currentPatrolIndex = 0; // ここでインデックスを0に設定
        MoveToNextPoint(); // 初めの移動ポイントへ
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        if (isDead) return; // 死んでいるなら処理しない
        if (player == null) return; // プレイヤーがいないときは処理をやめる

        // 現在のポイントに到達したら、停止して次のポイントへ移動
        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            waitCoroutine = StartCoroutine(WaitPoint()); // 開始時の探索処理
        }

        // 移動しているときに移動アニメーション
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

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

                if (isChangingState) return; // 状態遷移中は処理しない

                // 常にプレイヤーの方向を向く
                // プレイヤーの方向を取得
                Vector3 direction = player.position - transform.position;
                direction.y = 0; // Y軸を無視

                // 現在の向きとプレイヤー方向を補間して回転
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, targetFollowSpeed * Time.deltaTime);

                // プレイヤーが索敵外になったら
                if (!CanSearchPlayer() && !isChangingState)
                {
                    // 索敵モード
                    StartCoroutine(ChangeStateDelay(Mode.SearchState));
                    Debug.Log("プレイヤー見失った");
                }
                break;
        }
    }

    // 状態遷移を一定時間後に行う
    IEnumerator ChangeStateDelay(Mode newMode)
    {
        // 射撃用コルーチンが動いていたら
        if(fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine); // 射撃用コルーチンを止める
            fireCoroutine = null; // 値をnullにする
        }
        else if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine); // 停止用コルーチンを止める
            waitCoroutine = null;
        }

        isChangingState = true;
        if (newMode == Mode.BattleState) // バトル状態の設定時
        {
            // 戦闘状態に切り替わる時間
            yield return new WaitForSeconds(battleModeTransTime);

            // 攻撃アニメ開始
            animator.SetBool("Attack", true);

            fireCoroutine = StartCoroutine(FireCoroutine()); // 発射処理
        }
        else if (newMode == Mode.SearchState) // 探索状態の設定時
        {
            // 攻撃アニメ終了
            animator.SetBool("Attack", false);

            // 探索状態に切り替わる時間
            yield return new WaitForSeconds(searchModeTransTime);

            agent.isStopped = false; // 移動開始
            currentPatrolIndex -= 1; // 探索復帰時にマイナスにする
            MoveToNextPoint(); // 開始時の探索処理
        }
        stateMode = newMode;
        isChangingState = false;
    }

    // 弾を撃ち続けるコルーチン
    IEnumerator FireCoroutine()
    {
        float interval = 1f / fireRate; // 1秒をfireRateで割った時間間隔

        while (true) // 無限ループで撃ち続ける
        {
            Fire(); // 発射処理
            yield return new WaitForSeconds(interval); // 一定間隔で発射
        }
    }

    // 弾を撃つ処理
    void Fire()
    {
        if (isDead) return; // 死んでいるなら処理しない

        // 自身から弾を生成
        Instantiate(bulletPrefab, bullet.transform.position, transform.rotation);
        Debug.Log("敵が弾を発射");
    }

    IEnumerator WaitPoint()
    {
        // 現在のパトロールポイントの待機時間
        float waitTime = waitTimes[currentPatrolIndex];

        agent.isStopped = true; // 移動を停止
        yield return new WaitForSeconds(waitTime); // 設定した時間待機

        agent.isStopped = false; // 移動再開
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

        // 次の目的地へ
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPos.Length; // ループする
    }

    // プレイヤーの判定
    bool CanSearchPlayer()
    {
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
        if(isDead) return; // 死んでいる時は処理しない

        // ダメージ処理
        currentHP -= damage;
        if (currentHP <= 0.0f)
        {
            // モーショントリガー
            animator.SetTrigger("Death");
            currentHP = 0.0f; // 0以下にしない
            Die(); // 死んだ処理
        }
        else
        {
            // モーショントリガー
            animator.SetTrigger("TakeDamage");
        }

        Debug.Log("TakeDamageが呼ばれた!!");
    }

    // 死んだときの処理
    void Die()
    {
        isDead = true; // 死んだときのフラグ
        Invoke("AfterDestroy", 1.0f);
    }

    // 削除
    void AfterDestroy()
    {
        Destroy(gameObject);
    }

    // デバッグ可視化用
    void OnDrawGizmos()
    {
        if (isSearchDebug)
        {
            // 索敵範囲
            Gizmos.color = UnityEngine.Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchDistance);

            // 視野角の表示
            Vector3 leftBoundary = Quaternion.Euler(0, -searchOfView / 2, 0) * transform.forward * searchDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, searchOfView / 2, 0) * transform.forward * searchDistance;

            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }

        if (isButtleDebug)
        {
            // 索敵範囲
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawWireSphere(transform.position, battleDistance);

            // 視野角の表示
            Vector3 leftBoundary = Quaternion.Euler(0, -battleOfView / 2, 0) * transform.forward * battleDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, battleOfView / 2, 0) * transform.forward * battleDistance;

            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }
    }
}
