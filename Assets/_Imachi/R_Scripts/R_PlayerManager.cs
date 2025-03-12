using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// プレイヤーを管理するクラス
/// </summary>
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

	//パラメーター
	[SerializeField, Header("プレイヤーが移動する速度")] float _moveSpeed = 2.0f;
	[SerializeField, Header("プレイヤーのHP")] float _HP;
	[SerializeField, Header("プレイヤーがリロードなしで発射できる最大弾数")] float _bulletSpeed;

	float _speed​​Compensation;	//速度補正
	Vector3 _moveDir;			//移動方向
	Transform _transform;       //キャッシュ用
	float _time;                //デルタタイム格納用


	enum ACTION
	{
		ATTACK,
		DEFENCE,
		RELOAD
	}

	ACTION _actionType;
	#endregion



	void Start()
	{
		_transform = transform;
		_time = Time.deltaTime;
	}


	void Update()
	{

		//移動速度補正
		switch (_actionType)
		{
			case ACTION.ATTACK:
				_speed​​Compensation = 0.5f;
				break;
			case ACTION.DEFENCE:
				_speed​​Compensation = 0.25f;
				break;
			case ACTION.RELOAD:
				_speed​​Compensation = 0.5f;
				break;
			default://通常時
				_speed​​Compensation = 1.0f;
				break;
		}


		_PlayerMove();
	}



	/// <summary>
	/// プレイヤーの移動処理
	/// </summary>
	void _PlayerMove()
	{
		//何も入力が無い場合は方向初期化
		_moveDir = Vector3.zero;

		//正面方向へ加算
		if (Input.GetKey(KeyCode.W)) _moveDir += Vector3.forward;

		//左方向へ加算
		if (Input.GetKey(KeyCode.A)) _moveDir += Vector3.left;

		//右方向へ加算
		if (Input.GetKey(KeyCode.D)) _moveDir += Vector3.right;

		//後ろ方向へ加算
		if (Input.GetKey(KeyCode.S)) _moveDir += Vector3.back;


		//移動キーが押されていた場合
		if (_moveDir != Vector3.zero)
		{
			//移動の正規化
			_moveDir = _moveDir.normalized; 

			//移動処理
			_transform.position += _moveDir * _moveSpeed * _speedCompensation * _time;

			//方向に応じた回転処理
			_transform.rotation = Quaternion.LookRotation(_moveDir);
		}
	}
}
