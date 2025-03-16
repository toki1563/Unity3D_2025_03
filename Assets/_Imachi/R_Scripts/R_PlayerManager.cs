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

	#region ENUM

	/// <summary>
	/// 現在のプレイヤーの行動タイプ 移動中はdefault
	/// </summary>
	enum ACTION
	{
		ATTACK,DEFENCE,RELOAD
	}

	/// <summary>
	/// 現在の押された移動キー
	/// </summary>
	enum INPUT
	{
		W_KEY, A_KEY, S_KEY, D_KEY, NONE
	}

	#endregion

	#region 変数

	//パラメーター
	[SerializeField, Header("プレイヤーが移動する速度")] float _moveSpeed = 2.0f;
	[SerializeField, Header("プレイヤーのHP")] float _HP;
	[SerializeField, Header("プレイヤーがリロードなしで発射できる最大弾数")] float _maxBulletCount;

	float _speed​​Compensation;	//速度補正
	Vector3 _moveDir;			//移動方向
	Transform _transform;       //キャッシュ用
	float _time;                //デルタタイム格納用

	ACTION _actionType;
	List<INPUT> _pressedInput = new List<INPUT>();	//押されている方向キーを格納する　何も押されていないときは空

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

		//何も入力が無い場合は方向・リスト初期化
		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			_moveDir = Vector3.zero;
			_pressedInput.Clear();
		}


		//正面方向のキーが押されたとき リストに追加
		if (Input.GetKeyDown(KeyCode.W)) _pressedInput.Add(INPUT.W_KEY);

		//後ろ方向のキーが押されたとき リストに追加
		if (Input.GetKeyDown(KeyCode.S)) _pressedInput.Add(INPUT.S_KEY);

		//左方向のキーが押されたとき リストに追加
		if (Input.GetKeyDown(KeyCode.A)) _pressedInput.Add(INPUT.A_KEY);

		//右方向のキーが押されたとき リストに追加
		if (Input.GetKeyDown(KeyCode.D)) _pressedInput.Add(INPUT.D_KEY);



		//リストの０番目と１番目が埋まったら、0番目を削除して更新
		if (_pressedInput.Count > 2)
		{
			_pressedInput.RemoveAt(0);
		}


		//リスト０番目、１番目の移動方向を加算する
		foreach (INPUT key in _pressedInput)
		{
			switch (key)
			{
				case INPUT.W_KEY: 
					_moveDir += Vector3.forward;

					//逆方向のキーが押された場合リストの最後のキーを優先
					if (_pressedInput[0] == INPUT.S_KEY) _moveDir += Vector3.forward;
					break;

				case INPUT.A_KEY:
					_moveDir += Vector3.left;

					//逆方向のキーが押された場合リストの最後のキーを優先
					if (_pressedInput[0] == INPUT.D_KEY) _moveDir += Vector3.left;
					break;

				case INPUT.S_KEY:
					_moveDir += Vector3.back;

					//逆方向のキーが押された場合リストの最後のキーを優先
					if (_pressedInput[0] == INPUT.W_KEY) _moveDir += Vector3.back;
					break;

				case INPUT.D_KEY:
					_moveDir += Vector3.right;

					//逆方向のキーが押された場合リストの最後のキーを優先
					if (_pressedInput[0] == INPUT.A_KEY) _moveDir += Vector3.right;
					break;
			}
		}


		//移動キーが押されていた場合
		if (_moveDir != Vector3.zero)
		{
			//移動の正規化
			_moveDir = _moveDir.normalized; 

			//移動処理
			_transform.position += _moveDir * _moveSpeed * _speedCompensation * _time;

			//移動方向に回転する
			_transform.rotation = Quaternion.LookRotation(_moveDir);
		}
	}
}
