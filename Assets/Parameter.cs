using UnityEngine;

/// <summary>
/// パラメーター管理クラス
/// </summary>
public class Parameter : MonoBehaviour, IDamageable
{
    public enum State
    {
        None,
        Arive,
        Death
    }

    [SerializeField]
    private int _MaxHP;

    /// <summary>現在のHP</summary>
    private int _CurrentHP;
    /// <summary>現在のステート</summary>
    private State _CurrentState;

    /// <summary>現在のHPを取得</summary>
    public int GetCurrentHP => _CurrentHP;
    /// <summary>現在のステートを取得</summary>
    public State CurrentState => _CurrentState;

    private void Awake()
    {
        _CurrentHP = _MaxHP;
        ChengeState(State.Death);
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
    /// ステートの変更をする
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
