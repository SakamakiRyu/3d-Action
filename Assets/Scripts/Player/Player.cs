using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Playerクラス</summary>
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class Player : CharactorBase
{
    /// <summary>このクラスのインスタンスがあるかのフラグ</summary>
    static bool m_isInstance = false;

    /// <summary>ジャンプ力</summary>
    [SerializeField]
    float m_jumpPower = default;

    /// <summary>振り向く速度</summary>
    [SerializeField]
    float m_turnSpeed = default;

    /// <summary>現在のEP</summary>
    [SerializeField]
    int m_currentMp = default;

    /// <summary>最大EP</summary>
    [SerializeField]
    int m_maxMp = 100;

    /// <summary>地面と接地しているか</summary>
    bool m_isGrounded = true;

    Rigidbody m_rb;
    InputAction m_move, m_jump;

    private void Awake()
    {
        if (m_isInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            m_isInstance = true;
            CurrentHp = m_maxHp;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_move = GetComponent<PlayerInput>().currentActionMap["Move"];
        m_jump = GetComponent<PlayerInput>().currentActionMap["Jump"];
    }

    void Update()
    {
        Move();
        Jump();
    }

    public override void Move()
    {
        Vector2 v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * v2.y + Vector3.right * v2.x;

        if (dir == Vector3.zero)
        {
            if (m_isGrounded)
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
            if (m_isGrounded)
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

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
    }

    public override int SendAtkPower()
    {
        int atkPower = 0;
        atkPower += m_atk;
        return atkPower;
    }

    /// <summary>飛ぶ</summary>
    void Jump()
    {
        if (m_jump.triggered && m_isGrounded)
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }

    #region 接地判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            m_isGrounded = true;
            Debug.Log("接地");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            m_isGrounded = false;
            Debug.Log("離地");
        }
    }
    #endregion
}