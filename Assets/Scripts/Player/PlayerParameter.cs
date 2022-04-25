using UnityEngine;

/// <summary>
/// プレイヤーにパラメーターを付与するクラス
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
    /// <summary>現在のHPを取得</summary>
    public int GetCurrentHP => m_currentHP;
    /// <summary>現在のステートを取得</summary>
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
    /// 体力を減らす
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
    /// ステートの変更をする
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
    /// ステート毎に毎フレーム呼ばれる処理
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
