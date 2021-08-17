using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine
{
    /// <summary>ジャンプ状態</summary>
    public class PlayerStateJump : PlayerStateBase
    {
        public override void OnEnter(PlayerStateMachine owner)
        {
            owner.m_rb.AddForce(Vector3.up * owner.m_jumpPower, ForceMode.Impulse);
        }

        public override void OnUpdate(PlayerStateMachine owner)
        {
            if (owner.IsGrounded())
            {
                owner.ChengeState(m_stateIdle);
            }
        }
    }
}