using UnityEngine;

/// <summary>
/// �v���C���[�Ƀp�����[�^�[��t�^����N���X
/// </summary>
public class PlayerParameter : MonoBehaviour, IDamageable
{
    public enum State
    {
        None,
        Arive,
        Death
    }

    [SerializeField]
    private int _MaxHP;

    /// <summary>���݂�HP</summary>
    private int _CurrentHP;
    /// <summary>���݂̃X�e�[�g</summary>
    private State _CurrentState;

    /// <summary>���݂�HP���擾</summary>
    public int GetCurrentHP => _CurrentHP;
    /// <summary>���݂̃X�e�[�g���擾</summary>
    public State GetCurrentState => _CurrentState;

    private void Awake()
    {
        _CurrentHP = _MaxHP;
        ChengeState(State.Arive);
    }

    public void AddDamage(int damage)
    {
        var after = _CurrentHP - damage;

        if (after <= 0)
        {
            ChengeState(State.Death);
            return;
        }

        _CurrentHP = after;
    }

    /// <summary>
    /// �X�e�[�g�̕ύX������
    /// </summary>
    private void ChengeState(State next)
    {
        var prev = _CurrentState;

        switch (next)
        {
            case State.None:
                { }
                break;
            case State.Arive:
                { }
                break;
            case State.Death:
                { }
                break;
        }

        _CurrentState = next;
    }
}
