using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine
{
    /// <summary>通常状態</summary>
    public class PlayerStateIdle : PlayerStateBase
    {
        public override void OnUpdate(PlayerStateMachine owner)
        {
            if (owner.transform.position != Vector3.zero)
            {
                owner.ChengeState(m_stateWalk);
            }
            if (owner.m_jump.triggered)
            {
                owner.ChengeState(m_stateJump);
            }
        }
    }
}
