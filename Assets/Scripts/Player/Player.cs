using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>Playerクラス</summary>
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class Player : CharactorBase
{
    #region メンバー変数
    /// <summary>このクラスのインスタンスがあるかのフラグ</summary>
    static bool m_IsInstance = false;

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
    /// <summary>接地しているか否か</summary>
    bool m_IsGrounded = true;
    /// <summary>モーション中か否か</summary>
    bool m_IsPlayMotion = false;
    /// <summary>Event</summary>
    public Action m_damaged, m_useSkill;

    Vector2 v2;
    Rigidbody m_rb;
    InputAction m_move, m_jump, m_attack, m_skill;
    Animator m_anim;

    #endregion

    private void Awake()
    {
        if (m_IsInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            m_IsInstance = true;
            DontDestroyOnLoad(gameObject);
        }
    }

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
    }

    private void LateUpdate()
    {
        Vector3 v3 = m_rb.velocity;
        v3.y = 0;
        if (m_anim)
        {
            m_anim.SetFloat("Speed", v3.magnitude);
            m_anim.SetBool("IsGrounded", m_IsGrounded);
            m_anim.SetBool("IsPlayMotion", m_IsPlayMotion);
            m_anim.SetFloat("PlayWalkSpeed", v2.magnitude);
        }
    }

    /// <summary>InputSystemの読み込み</summary>
    void LoadInputSystem()
    {
        m_rb = GetComponent<Rigidbody>();
        m_move = GetComponent<PlayerInput>().currentActionMap["Move"];
        m_jump = GetComponent<PlayerInput>().currentActionMap["Jump"];
        m_attack = GetComponent<PlayerInput>().currentActionMap["Attack"];
        m_skill = GetComponent<PlayerInput>().currentActionMap["Skill"];
    }

    /// <summary>Playerの移動</summary>
    public override void Move()
    {
        v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * v2.y + Vector3.right * v2.x;

        if (dir == Vector3.zero)
        {
            if (m_IsGrounded)
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
            if (m_IsGrounded && !m_IsPlayMotion) // 地上にいるかつ、モーション中じゃなければ移動する
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
        Debug.Log("Sound");
    }

    /// <summary>ダメージを受ける</summary>
    /// <param name="damage">ダメージ量</param>
    public override void Damaged(int damage)
    {
        CurrentHP -= damage;
        m_damaged?.Invoke();
    }

    public override int SendAtkPower()
    {
        int atkPower = 0;
        atkPower += m_atk;
        return atkPower;
    }

    public void SetDate(int hp, int ep)
    {
        CurrentHP = hp;
        CurrentEP = ep;
    }

    void Jump()
    {
        if (m_jump.triggered && m_IsGrounded && !m_IsPlayMotion)
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>攻撃</summary>
    void Attack()
    {
        // 地上に接地しているかつ、静止モーション中じゃない場合にスキルか攻撃が可能になる
        if (m_IsGrounded && !m_IsPlayMotion)
        {
            if (m_skill.triggered && CurrentEP > 20)
            {
                CurrentEP -= 20;
                m_anim.SetTrigger("Skill");
                m_useSkill?.Invoke();
                return;
            }
            if (m_attack.triggered)
            {
                m_anim.SetTrigger("Attack1");
            }
        }
    }

    #region 接地判定 {自分以外の何かしらに接触していたら接地しているとみなす(仮)}
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != this.gameObject)
        {
            m_IsGrounded = true;
            Debug.Log("接地");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != this.gameObject)
        {
            m_IsGrounded = false;
            Debug.Log("離地");
        }
    }
    #endregion
}