using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_PlayerManager : MonoBehaviour
{
	#region �V���O���g��
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
        //�C���X�^���X�����ɑ��݂��Ă����玩�g����������
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
    }
	#endregion


    #region �ϐ�

	//�v���C���[���g
	[SerializeField,Header("�ړ����x")] float _moveSpeed = 2.0f;
	[SerializeField,Header("��]���x")] float _rotSpeed = 1.0f;
    Transform _transform;       //�L���b�V���p
    float _time;                //�f���^�^�C���i�[�p

    //�e
    [SerializeField, Header("�e���ˑ��x")] float _bulletSpeed;

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
    /// �v���C���[�̈ړ�����
    /// </summary>
    void _PlayerMove()
	{
        //���ʈړ�
        if (Input.GetKey(KeyCode.W))
        {
            _transform.position += transform.forward * _moveSpeed * _time;
        }

        //���ɕ����ϊ�
        if (Input.GetKey(KeyCode.A))
        {
            _transform.Rotate(0, -_rotSpeed, 0);
        }

        //�E�ɕ����ϊ�
        if (Input.GetKey(KeyCode.D))
        {
            _transform.Rotate(0, _rotSpeed, 0);
        }

        //���ʌ��
        if (Input.GetKey(KeyCode.S))
        {
            _transform.position += -transform.forward * _moveSpeed * _time;
        }
    }
}
