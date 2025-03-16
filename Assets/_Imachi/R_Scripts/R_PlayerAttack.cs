using UnityEngine;
using System.Collections;

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
	int _currentBulletCount;							//現在の弾数
	float _bulletFireInterval = 0.2f;					//発射間隔
	bool _canBullet = true;								//True：発射可能
	bool _canReload = false;							//True：リロード可能

	public bool _CanBullet { get => _canBullet;}
	public bool _CanReload { get => _canReload;}



	private void Start()
	{
		_currentBulletCount = R_PlayerManager.Instance._MaxBulletCount;
	}

	private void Update()
	{		
		//弾が０以下になったらリロード可能にする
		if (_currentBulletCount <= 0) _canReload = true;
	}


	/// <summary>
	/// スペースキーが押されたとき
	/// 発射可能なら攻撃処理実行
	/// </summary>
	public void _StartAttack()
	{
		if (!_canBullet && _currentBulletCount <= 0) return;

		//発射間隔を越えたら発射可能
		StartCoroutine(_PlayerAttack(_bulletFireInterval));
	}


	/// <summary>
	/// プレイヤーの攻撃処理
	/// </summary>
	public IEnumerator _PlayerAttack(float _reloadInterval)
	{
		_currentBulletCount--;
		_canBullet = false;

		//弾生成
		GameObject bulletObj = Instantiate(R_PlayerManager.Instance._BulletPrefab, R_PlayerManager.Instance._BulletShotTran.position,Quaternion.identity);
		_rb = bulletObj.GetComponent<Rigidbody>();

		//発射
		Vector3 bulletDir = R_PlayerManager.Instance._PlayerDir;
		_rb.AddForce(bulletDir * R_PlayerManager.Instance._BulletPower, ForceMode.Impulse);

		Destroy(bulletObj, 1.0f);

		//発射間隔を測る
		yield return new WaitForSeconds(_reloadInterval);

		//初期化
		_canBullet = true;
		 R_PlayerManager.Instance._ActionType = R_PlayerManager.ACTION.DEFAULT;
	}


	/// <summary>
	/// プレイヤーのリロード処理
	/// 弾が０以下でRキーを押したら実行
	/// <param name="_reloadInterval">リロード完了速度</param>
	/// </summary>
	public void _StartReload(float _reloadInterval)
	{
		if (!_canReload) return;
		StartCoroutine(_PlayerReload(_reloadInterval));
	}

	/// <summary>
	/// リロード完了速度中に他のアクションをしたら中断する（※未実装）
	/// </summary>
	/// <param name="_reloadInterval">リロード完了速度</param>
	/// <returns></returns>
	public IEnumerator _PlayerReload(float _reloadInterval)
	{
		//リロード完了速度
		yield return new WaitForSeconds(_reloadInterval);

		//初期化
		_canReload = false;
		_currentBulletCount = R_PlayerManager.Instance._MaxBulletCount;
		R_PlayerManager.Instance._ActionType = R_PlayerManager.ACTION.DEFAULT;
	}
}
