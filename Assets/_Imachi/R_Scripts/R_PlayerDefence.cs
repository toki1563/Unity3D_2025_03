using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_PlayerDefence : MonoBehaviour
{
	#region シングルトン
	static R_PlayerDefence instance;

	public static R_PlayerDefence Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<R_PlayerDefence>();
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


	float _defencePower = 0.5f;



    public void _PlayerDefence()
    {
		R_PlayerManager.Instance._Defence = _defencePower;
	}
}
