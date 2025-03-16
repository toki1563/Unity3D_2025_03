using UnityEngine;

/// <summary>
/// プレイヤー全体を管理するクラス
/// </summary>
public class R_PlayerManager : MonoBehaviour
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
	[SerializeField] Transform _bulletShotTran;

	[Header("パラメータ")]
	[SerializeField, Header("プレイヤーが移動する速度")] float _moveSpeed;
	[SerializeField, Header("プレイヤーのHP")] float _HP;
	[SerializeField, Header("プレイヤーがリロードなしで発射できる最大弾数")] int _maxBulletCount;
	[SerializeField, Header("プレイヤーの弾の速度")] float _bulletSpeed;
	[SerializeField, Header("プレイヤーのリロードの速度")] float _bulletReloadSpeed;


	[SerializeField]ACTION _actionType = ACTION.DEFAULT;					//行動タイプ
	Transform _transform;									//キャッシュ用
	float _time;											//デルタタイム格納用
	Vector3 _playerDir;                                     //方向
	float _defence = 1.0f;

	public ACTION _ActionType { get => _actionType; set => _actionType = value; }
	public Transform _Transform { get => _transform; set => _transform = value; }
	public Vector3 _PlayerDir { get => _playerDir; set => _playerDir = value; }
	public float _Time { get => _time;}
	public float _MoveSpeed { get => _moveSpeed;}
	public GameObject _BulletPrefab { get => _bulletPrefab;}
	public Transform _BulletShotTran { get => _bulletShotTran;}
	public float _BulletSpeed { get => _bulletSpeed; }
	public int _MaxBulletCount { get => _maxBulletCount;}
	public float _Defence { get => _defence; set => _defence = value; }

	#endregion



	void Start()
	{
		_transform = transform;
		_time = Time.deltaTime;
		_playerDir = Vector3.forward;
	}


	void Update()
	{
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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (R_PlayerAttack.Instance._CanReload)
			{
				_actionType = ACTION.RELOAD;
				R_PlayerAttack.Instance._StartReload(_bulletReloadSpeed);
			}
		}
		else if(Input.GetKeyDown(KeyCode.R))
		{
			_actionType = ACTION.RELOAD;
			R_PlayerAttack.Instance._StartReload(_bulletReloadSpeed);
		}

		//防御
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			_actionType = ACTION.DEFENCE;
			R_PlayerDefence.Instance._PlayerDefence();
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			//離されたら通常に戻す
			_actionType = ACTION.DEFAULT;
			_defence = 1.0f;
		}

	}




}
