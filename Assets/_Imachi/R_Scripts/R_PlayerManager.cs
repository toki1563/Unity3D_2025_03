using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤー全体を管理するクラス
/// </summary>
public class R_PlayerManager : MonoBehaviour, IDamage, IGameOverSender, IStageClearSender ,IGameStartSender
{
	#region シングルトン
	static R_PlayerManager instance;

	public static  R_PlayerManager Instance
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

	/// <summary>
	/// 現在のプレイヤーの行動タイプ 移動中はdefault
	/// </summary>
	public enum ACTION
	{
		ATTACK, DEFENCE, RELOAD, DEFAULT
	}


	[SerializeField] GameObject _bulletPrefab;
	[SerializeField] Transform _bulletShotTran;				//発射口

	[Header("パラメータ")]
	[SerializeField, Header("プレイヤーが移動する速度")] float _moveSpeed;
	[SerializeField, Header("プレイヤーの最大HP")] float _maxHP;
	[SerializeField, Header("プレイヤーがリロードなしで発射できる最大弾数")] int _maxBulletCount;
	[SerializeField, Header("プレイヤーの弾の速度")] float _bulletSpeed;


	ACTION _actionType = ACTION.DEFAULT;                    //行動タイプ
	Vector3 _playerDir;                                     //方向
	Transform _transform;									//キャッシュ用
	float _time;											//デルタタイム格納用
	float _defence = 1.0f;									//防御率
	float _currentHP;										//現在の体力
	float _flashDamageTime = 0.1f;                          //ダメージを受けたときの赤く光る時間
	int _currentBulletCount;                                //現在の弾数
	bool _isDead = false;                                   //True:プレイヤーがしぬ
	bool _isGameStart = false;

	public event Action SendGameOver;
	public event Action SendStageClear;
	public event Action SendGameStart;

	public ACTION _ActionType { get => _actionType; set => _actionType = value; }
	public Transform _Transform { get => _transform; set => _transform = value; }
	public Vector3 _PlayerDir { get => _playerDir; set => _playerDir = value; }
	public float _Time { get => _time;}
	public float _MoveSpeed { get => _moveSpeed;}
	public GameObject _BulletPrefab { get => _bulletPrefab;}
	public Transform _BulletShotTran { get => _bulletShotTran;}
	public float _BulletSpeed { get => _bulletSpeed; }
	public float _Defence { get => _defence; set => _defence = value; }


	//UIで使用する
	public float _CurrentHP { get => _currentHP;}
	public float _MaxHP { get => _maxHP;}
	public int _MaxBulletCount { get => _maxBulletCount; }
	public int _CurrentBulletCount { get => _currentBulletCount; set => _currentBulletCount = value; }

	#endregion



	void Start()
	{
		_transform = transform;
		_time = Time.deltaTime;
		_playerDir = Vector3.forward;
		_currentHP = _maxHP;
		_currentBulletCount = _maxBulletCount;
	}


	void Update()
	{
		//ゲームが始まるまでは実行しない
		if (!_isGameStart) return;

		//体力がなくなったらこれ以上処理しない
		if (_isDead) return;

		//移動
		R_PlayerMove.Instance._PlayerMove();

		//攻撃
		if (Input.GetKey(KeyCode.Space))
		{
			if (R_PlayerAttack.Instance._CanBullet && !R_PlayerAttack.Instance._CanReload)
			{
				_actionType = ACTION.ATTACK;
				R_PlayerAttack.Instance._StartAttack();
			}
		}

		//リロード
		if (Input.GetKeyDown(KeyCode.Space) && R_PlayerAttack.Instance._CanReload || Input.GetKeyDown(KeyCode.R))
		{
			if (R_PlayerAttack.Instance._CanReload)
			{
				//弾数が０の場合
				Debug.Log("残弾0でリロード");
				_actionType = ACTION.RELOAD;
				R_PlayerAttack.Instance._StartReload(2.0f);
			}
			else
			{
				//弾数が残っている場合
				Debug.Log("リロード");
				_actionType = ACTION.RELOAD;
				R_PlayerAttack.Instance._StartReload(1.0f);
			}
		}

		////リロード中に再度押されたらキャンセル
		//if (R_PlayerAttack.Instance._IsBulletReload && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
		//{
		//	R_PlayerAttack.Instance._IsBulletReload = false;
		//	//_actionType = ACTION.DEFAULT;
		//}


		//防御
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			_actionType = ACTION.DEFENCE;
			R_PlayerAttack.Instance._IsBulletReload = false;
			R_PlayerDefence.Instance._PlayerDefence();
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			//離されたら通常に戻す
			_actionType = ACTION.DEFAULT;
			_defence = 1.0f;
		}

	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("Goal"))
		{
			_GameClear();
		}
	}

	/// <summary>
	/// プレイヤーのダメージ処理
	/// </summary>
	/// <param name="damage">敵の弾の攻撃力</param>
	public void TakeDamage(int damage)
	{
		if (_isDead) return;

		//防御時はダメージ50％減
		if (_actionType == ACTION.DEFENCE) _currentHP -= damage * R_PlayerDefence.Instance._DefencePower;
		else _currentHP -= damage;

		//プレイヤーを赤くする
		StartCoroutine(_FlashRedDamegeColor(_flashDamageTime));

		//死んだ処理
		if (_currentHP < 0.0f) _Die();
	}

	/// <summary>
	/// ダメージをうけたらプレイヤーを指定した時間赤く光らせる
	/// </summary>
	/// <returns></returns>
	IEnumerator _FlashRedDamegeColor(float flashTime)
	{
		//色を赤くする
		gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0, 0, 1.0f);

		yield return new WaitForSeconds(flashTime);

		//元の色に戻す
		gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	void _GameStartSet()
	{
		SendGameStart.Invoke();
		_isGameStart = true;
	}

	/// <summary>
	/// プレイヤーの体力が０以下になったらゲームオーバー
	/// </summary>
	void _Die()
	{
		_isDead = true;
		SendGameOver.Invoke();
	}

	/// <summary>
	/// ゴールについたらゲームクリア
	/// </summary>
	void _GameClear()
	{
		if (_isDead) return;
		SendStageClear.Invoke();
	}
}
