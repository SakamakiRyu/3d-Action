using UnityEngine;

/// <summary>
/// �A�j���[�V�����̊Ǘ��N���X
/// </summary>
public class AnimationControl : MonoBehaviour
{
    [SerializeField]
    private Animator _Animator;

    private void Start()
    {
        if (_Animator is null) Debug.LogError("Animator is null !");
    }

    /// <summary>
    /// �A�j���[�V�����̕ύX������
    /// </summary>
    /// <param name="name">�X�e�[�g��</param>
    /// <param name="duration">���b��ɃA�j���[�V������؂�ւ��邩</param>
    public void ChengeAnimation(string name, float duration = 0.0f)
    {
        if (_Animator is null) return;

        _Animator.CrossFade(name, duration);
    }

    /// <summary>
    /// �A�j���[�V�����̍Đ����x��ύX����
    /// </summary>
    /// <param name="time">�Đ����x</param>
    public void ChengeAnimationSpeed(float time)
    {
        if (_Animator is null) return;

        _Animator.speed = time;
    }
}
