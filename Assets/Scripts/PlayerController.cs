using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Playerの操作クラス。</summary>
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody), typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = default;

    [SerializeField]
    float m_jumpPower = default;

    [SerializeField]
    float m_turnSpeed = default;

    [SerializeField, Tooltip("自身の中央(Pivot)から、真下に線を伸ばしオブジェクトが当たったら接地しているとみなす。その際に使用する線の長さ")]
    float m_lineLength = default;

    PlayerStatus m_pStatus;
    Rigidbody m_rb;
    InputAction m_move, m_jump;

    void Start()
    {
        m_pStatus = GetComponent<PlayerStatus>();
        m_rb = GetComponent<Rigidbody>();
        m_move = GetComponent<PlayerInput>().currentActionMap["Move"];
        m_jump = GetComponent<PlayerInput>().currentActionMap["Jump"];
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Attack(StatusBase target)
    {
        BattleManager.Instance.Battle(target, this.m_pStatus);
    }

    /// <summary>キャラクターの移動</summary>
    void Move()
    {
        Vector2 v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * v2.y + Vector3.right * v2.x;

        if (dir == Vector3.zero)
        {
            if (IsGrounded() == true)
            {
                m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
            }
            else
            {
                m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, m_rb.velocity.z);
            }
        }
        else
        {
            if (IsGrounded() == true)
            {
                //  カメラを基準に移動する
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                //  入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);

                Vector3 velo = dir * m_moveSpeed;
                velo.y = m_rb.velocity.y;
                m_rb.velocity = velo;
            }
        }
    }

    /// <summary>飛ぶ</summary>
    void Jump()
    {
        if (m_jump.triggered && IsGrounded())
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>接地判定</summary>
    bool IsGrounded()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * m_lineLength;
        Debug.DrawLine(start, end);
        bool isGrounded = Physics.Linecast(start, end);
        return isGrounded;
    }
}