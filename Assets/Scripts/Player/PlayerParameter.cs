using UnityEngine;

/// <summary>
/// �v���C���[�Ƀp�����[�^�[��t�^����N���X
/// </summary>
public class PlayerParameter : MonoBehaviour
{
    public enum State
    {
        None,
        Arive,
        Death
    }

    #region Serialized Field
    [SerializeField]
    private int m_maxHP;
    #endregion

    #region Private Field
    private int m_currentHP;
    private State m_currentState;
    #endregion

    #region Property
    /// <summary>���݂�HP���擾</summary>
    public int GetCurrentHP => m_currentHP;
    /// <summary>���݂̃X�e�[�g���擾</summary>
    public State GetCurrentState => m_currentState;
    #endregion

    private void Awake()
    {
        m_currentHP = m_maxHP;
    }

    private void Start()
    {
        ChengeState(State.Arive);
    }

    private void Update()
    {
        StateUpdate();
    }

    /// <summary>
    /// �̗͂����炷
    /// </summary>
    public void ReduceHP()
    {
        var after = --m_currentHP;

        if (after <= 0)
        {
            ChengeState(State.Death);
            return;
        }

        m_currentHP = after;
    }

    /// <summary>
    /// �X�e�[�g�̕ύX������
    /// </summary>
    private void ChengeState(State next)
    {
        var prev = m_currentState;

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

        m_currentState = next;
    }

    /// <summary>
    /// �X�e�[�g���ɖ��t���[���Ă΂�鏈��
    /// </summary>
    private void StateUpdate()
    {
        switch (m_currentState)
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
    }
}
