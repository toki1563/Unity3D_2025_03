using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �Q�[���X�^�[�g�E�Q�[���I�[�o�[�E�X�e�[�W�N���A�Ȃǂ̊Ǘ��i�e�V�X�e���̃C���^�[�t�F�[�X�j
/// </summary>
public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public UnityEvent OnStageClear;

    public void GameOver()
    {
        // �Q�[���I�[�o�[��ʒm
        OnGameOver.Invoke();

        // �Q�[���I�[�o�[���o�����ҋ@

        // ���݂̃V�[���������[�h
    }

    public void StageClear()
    {
        OnStageClear.Invoke();

        // ���o�����ҋ@

        // �V�[���J�ڏ���
    }
}
