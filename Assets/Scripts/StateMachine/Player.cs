using UnityEngine;

public partial class PlayerStateMachine
{
    /// <summary>Idle(State)</summary>
    private static readonly PlayerStateIdle m_stateIdle = new PlayerStateIdle();
    /// <summary>Walk(State)</summary>
    private static readonly PlayerStateWalk m_stateWalk = new PlayerStateWalk();
    /// <summary>Jump(State)</summary>
    private static readonly PlayerStateJump m_stateJump = new PlayerStateJump();

    /// <summary>現在のステート</summary>
    PlayerStateBase m_currentState = m_stateIdle;

    void OnStart()
    {
        m_currentState.OnEnter(this);
    }

    void OnUpdate()
    {
        m_currentState.OnUpdate(this);
    }

    void ChengeState(PlayerStateBase nextState)
    {
        m_currentState.OnExit(this);
        nextState.OnEnter(this);
        m_currentState = nextState;
    }
}
