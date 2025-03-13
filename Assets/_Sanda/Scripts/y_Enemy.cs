using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_Enemy : MonoBehaviour
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

    float currentHP = 0.0f; // 現在の体力
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


    }

    // 毎フレーム呼ばれる
    void Update()
    {
        switch (stateMode)
        {
            case Mode.SearchState: // 探索状態時

                break;
            case Mode.BattleState: // 戦闘状態時

                break;
        }
    }

    // 0.02秒毎に呼ばれる
    void FixedUpdate()
    {

    }
}
