using UnityEngine;

/// <summary>
/// プレイヤーの攻撃関係を管理するクラス
/// </summary>
public class R_PlayerAttack : MonoBehaviour
{
	#region シングルトン
	static R_PlayerAttack instance;

	public static R_PlayerAttack Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<R_PlayerAttack>();
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


	Rigidbody _rb;
	int _currentBulletCount;                    //現在の弾数
	[SerializeField]float _bulletFireInterval = 0.2f;           //発射間隔
	float _bulletIntervalCount = 0;				//現在の発射間隔タイマー
	bool _canBullet = false;					//True：発射可能
	bool _canReload = false;					//True：リロード可能

	public bool _CanBullet { get => _canBullet;}
	public bool _CanReload { get => _canReload;}



	private void Start()
	{
		_currentBulletCount = R_PlayerManager.Instance._MaxBulletCount;
	}

	private void Update()
	{
		//発射間隔を測る
		if (!_canBullet && _currentBulletCount > 0) _bulletIntervalCount += R_PlayerManager.Instance._Time;

		//発射間隔を越えたら発射可能
		if (_bulletIntervalCount > _bulletFireInterval && _currentBulletCount > 0) _canBullet = true;

		//弾が０以下になったらリロード可能にする
		if (_currentBulletCount <= 0) _canReload = true;
	}


	/// <summary>
	/// プレイヤーの攻撃処理
	/// スペースキーが押されたら実行
	/// </summary>
	public void _PlayerAttack()
	{
		R_PlayerManager.Instance._ActionType = R_PlayerManager.ACTION.ATTACK;
		_currentBulletCount--;

		//弾生成
		GameObject bulletObj = Instantiate(R_PlayerManager.Instance._BulletPrefab, R_PlayerManager.Instance._BulletShotTran.position,Quaternion.identity);
		_rb = bulletObj.GetComponent<Rigidbody>();

		//発射
		Vector3 bulletDir = R_PlayerManager.Instance._PlayerDir;
		_rb.AddForce(bulletDir * R_PlayerManager.Instance._BulletPower, ForceMode.Impulse);

		Destroy(bulletObj, 1.0f);


		//初期化
		_canBullet = false;
		_bulletIntervalCount = 0;
		R_PlayerManager.Instance._ActionType = R_PlayerManager.ACTION.DEFAULT;
	}

	/// <summary>
	/// プレイヤーのリロード処理
	/// 弾が０以下でスペースキーを押したら実行
	/// TODO：リロードに完了速度を追加する
	/// </summary>
	public void _PlayerReload()
	{
		if (!_canReload) return;

		_currentBulletCount = R_PlayerManager.Instance._MaxBulletCount;
		_canReload = false;
	}
}
