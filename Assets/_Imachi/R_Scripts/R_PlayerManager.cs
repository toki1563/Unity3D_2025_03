using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_PlayerManager : MonoBehaviour
{
	#region シングルトン
	static R_PlayerManager instance;

    public R_PlayerManager Instance
	{
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<R_PlayerManager>();
			}

            return instance;
        }

    }

    public void Awake()
    {
        //インスタンスが既に存在していたら自身を消去する
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
    }
	#endregion


    #region 変数

	//プレイヤー自身
	[SerializeField,Header("移動速度")] float _moveSpeed = 2.0f;
	[SerializeField,Header("回転速度")] float _rotSpeed = 1.0f;
    Transform _transform;       //キャッシュ用
    float _time;                //デルタタイム格納用

    //弾
    [SerializeField, Header("弾発射速度")] float _bulletSpeed;

    //HP
    [SerializeField, Header("体力")] float _HP;
	#endregion



	void Start()
    {
        _transform = transform;
        _time = Time.deltaTime;
    }


    void Update()
    {
        _PlayerMove();
    }



    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    void _PlayerMove()
	{
        //正面移動
        if (Input.GetKey(KeyCode.W))
        {
        }

        //左移動
        if (Input.GetKey(KeyCode.A))
        {
        }

        //右移動
        if (Input.GetKey(KeyCode.D))
        {
        }

        //後ろ移動
        if (Input.GetKey(KeyCode.S))
        {
        }
    }
}
