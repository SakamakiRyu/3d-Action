using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>Playerクラス</summary>
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class Player : CharactorBase
{
    #region メンバー変数
    [Header("接地判定")]

    [SerializeField, Tooltip("接地判定に使う球を配置する場所")]
    Transform m_isGroundedTrasnform = default;

    [SerializeField, Tooltip("接地判定に使う球の大きさ")]
    float m_isGroundedBoxSize = default;

    [Header("内部データ")]

    [SerializeField, Tooltip("ジャンプ力 (float)")]
    float m_jumpPower = default;

    [SerializeField, Tooltip("振り向く速さ (float)")]
    float m_turnSpeed = default;

    [Header("　　　　能力値パラメータ")]

    [SerializeField, Tooltip("HPの上限値 (int)")]
    int m_maxHp = 100;
    public int MaxHP => m_maxHp;

    [SerializeField, Tooltip("EPの上限値 (int)")]
    int m_maxEp = 100;
    public int MaxEP => m_maxEp;

    [SerializeField, Tooltip("攻撃力 (int)")]
    int m_atk = default;

    /// <summary>現在のEP</summary>
    public int CurrentEP { get; private set; }
    /// <summary>モーション中か否か</summary>
    bool m_IsPlayMotion = false;
    /// <summary>Event</summary>
    public Action Damaged, UseSkill;

    Vector2 v2;
    Rigidbody m_rb;
    InputAction m_move, m_jump, m_attack1, m_attack2, m_skill;
    Animator m_anim;

    #endregion

    void Start()
    {
        LoadInputSystem();
        m_anim = GetComponent<Animator>();
        CurrentHP = m_maxHp; // 後に削除し、ロードする
        CurrentEP = m_maxEp; // 
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
        IsGrounded();
    }

    private void LateUpdate()
    {
        Vector3 v3 = m_rb.velocity;
        v3.y = 0;
        if (m_anim)
        {
            m_anim.SetFloat("Speed", v3.magnitude);
            m_anim.SetBool("IsGrounded", !IsGrounded());
            m_anim.SetFloat("PlayWalkSpeed", v2.magnitude);
        }
    }

    /// <summary>InputSystemの読み込み</summary>
    void LoadInputSystem()
    {
        m_rb = GetComponent<Rigidbody>();
        m_move = GetComponent<PlayerInput>().currentActionMap["Move"];
        m_jump = GetComponent<PlayerInput>().currentActionMap["Jump"];
        m_attack1 = GetComponent<PlayerInput>().currentActionMap["Attack1"];
        m_attack2 = GetComponent<PlayerInput>().currentActionMap["Attack2"];
        m_skill = GetComponent<PlayerInput>().currentActionMap["Skill"];
    }

    /// <summary>Playerの移動</summary>
    public override void Move()
    {
        v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * v2.y + Vector3.right * v2.x;

        if (dir == Vector3.zero)
        {
            m_rb.velocity = !IsGrounded() ? new Vector3(0f, m_rb.velocity.y, 0f) : m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, m_rb.velocity.z);
        }
        else
        {
            if (!IsGrounded() && !m_IsPlayMotion) // 地上にいるかつ、モーション中じゃなければ移動する
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

    public enum IsMotion { True, False }

    /// <summary>アクションフラグを切り替える</summary>
    public void OnStateChenger(IsMotion isMotion)
    {
        if (isMotion == IsMotion.True)
        {
            m_IsPlayMotion = true;
            m_rb.velocity = Vector3.zero;
        }
        else
        {
            m_IsPlayMotion = false;
        }
    }

    /// <summary>足音を鳴らす(AnimationEventで呼ぶ)</summary>
    public void OnPlayFootSound()
    {

    }

    /// <summary>ダメージを受ける</summary>
    public override void GetDamage(int damage)
    {
        CurrentHP -= damage;
        Damaged?.Invoke();
    }

    public override int SendAtkPower()
    {
        int atkPower = 0;
        atkPower += m_atk;
        return atkPower;
    }

    void Jump()
    {
        if (m_jump.triggered && !IsGrounded() && !m_IsPlayMotion)
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }

    void Attack()
    {
        // 地上に接地しているかつ、静止モーション中じゃない場合にスキルか攻撃が可能になる
        if (!IsGrounded())
        {
            if (m_skill.triggered && CurrentEP > 20 && !m_IsPlayMotion)
            {
                CurrentEP -= 20;
                m_anim.SetTrigger("Skill");
                UseSkill?.Invoke();
                return;
            }
            if (m_attack1.triggered)
            {
                m_anim.SetTrigger("Attack1");
            }
            if (m_attack2.triggered)
            {
                m_anim.SetTrigger("Attack2");
            }
        }
    }

    bool IsGrounded()
    {
        bool isGrounded = Physics.BoxCast(m_isGroundedTrasnform.position, Vector3.one * m_isGroundedBoxSize, Vector3.down);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(m_isGroundedTrasnform.position, Vector3.one * m_isGroundedBoxSize);
    }

    public void SetDate(int hp, int ep)
    {
        CurrentHP = hp;
        CurrentEP = ep;
    }
}