using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystemの入力でHumanoidModelを動かすコンポーネント
/// 移動はカメラを基準にの相対的な移動をする。(常にカメラ前方が正面になる)
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class PlayerController : MonoBehaviour, IDamageable
{
    enum MoveState 
    {
        Idle,
        Run,
        Attack
    }

    #region Serialize Field
    [SerializeField]
    PlayerInput m_pInput = default;

    [SerializeField]
    Rigidbody m_rigidBody = default;

    [SerializeField]
    Animator m_animator = default;

    [SerializeField]
    Transform m_groundCheckTrasnform = default;

    [SerializeField]
    Collider m_attackCollider = default;

    [SerializeField]
    TrailRenderer m_trail = default;

    [SerializeField]
    int m_maxHP = default;

    [SerializeField]
    float m_moveSpeed = default;

    [SerializeField]
    float m_turnSpeed = default;

    [SerializeField]
    float m_jumpPower = default;
    #endregion

    #region Private Field
    int m_currentHP = default;
    bool m_isGrounded = false;
    bool m_isGameEnded = false;
    bool m_isMotionPlay = false;

    /// <summary>生きているか</summary>
    public bool IsArive => m_currentHP > 0;
    // アニメーターのハッシュ
    readonly int m_hashDamaged = Animator.StringToHash("Damaged");
    // インプットシステムの入力の取得
    InputAction m_move, m_attack, m_jump;

    AudioSource m_source;
    Vector2 m_v2;
    #endregion

    #region Unity Function
    void Awake()
    {
        m_currentHP = m_maxHP;
    }

    void Start()
    {
        TryGetComponent(out m_source);
        SetInput();
    }

    void Update()
    {
        if (!IsArive || m_isGameEnded) return;
        Walk();
        Jump();
        Attack();
    }

    void LateUpdate()
    {
        Vector3 v3 = m_rigidBody.velocity;
        v3.y = 0;
        m_animator.SetFloat("Speed", v3.magnitude);
        m_animator.SetFloat("AnimationSpeed", m_v2.magnitude);
        m_animator.SetBool("IsGrounded", m_isGrounded);
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            m_isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            m_isGrounded = false;
        }
    }
    #endregion

    #region Public Function
    // AnimationEvent用列挙
    public enum IsVisible { True, False }

    public void OnBeginTrail(IsVisible isVisible)
    {
        if (isVisible == IsVisible.False)
        {
            m_trail.enabled = false;
        }
        else
        {
            m_trail.enabled = true;
        }
    }
    public void OnPlayMotion()
    {
        m_isMotionPlay = true;
    }

    public void OnEndMotion()
    {
        m_isMotionPlay = false;
    }

    public void BeginAttack()
    {
        m_attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        m_attackCollider.enabled = false;
    }

    public void AddDamage(int damage)
    {
        // 既に死んでいたら下記の処理はしない。
        if (!IsArive) return;

        m_currentHP--;

        if (!IsArive)
        {
            m_animator.SetTrigger("Die");
            m_rigidBody.isKinematic = true;
            Mission.Instance.IsGameover = true;
            return;
        }
        m_animator.Play(m_hashDamaged);
    }

    /// <summary>イベントに登録</summary>
    public void Register()
    {
        Mission.Instance.OnGameEnd += OnEnd;
    }

    /// <summary>AnimationEvent用関数</summary>
    public void OnEnd()
    {
        m_isGameEnded = true;
    }

    /// <summary>AnimationEvent用関数</summary>
    /// <param name="clip"></param>
    public void OnPlaySound(AudioClip clip)
    {
        m_source.PlayOneShot(clip);
    }
    #endregion

    #region Private Function

    /// <summary>インプットシステムの入力を取得</summary>
    void SetInput()
    {
        m_move = m_pInput.currentActionMap["Move"];
        m_jump = m_pInput.currentActionMap["Jump"];
        m_attack = m_pInput.currentActionMap["Attack1"];
    }

    void Attack()
    {
        if (m_attack.triggered)
        {
            m_animator.SetTrigger("Attack1");
        }
    }

    /// <summary>歩く</summary>
    void Walk()
    {
        m_v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * m_v2.y + Vector3.right * m_v2.x;

        if (dir == Vector3.zero)
        {
            if (m_isGrounded)
            {
                m_rigidBody.velocity = new Vector3(0f, m_rigidBody.velocity.y, 0f);
            }
            else
            {
                m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, m_rigidBody.velocity.y, m_rigidBody.velocity.z);
            }
        }
        else
        {
            // 地面と接地していれば移動可能
            if (m_isGrounded)
            {
                //  カメラを基準に移動する
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                //  入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);

                Vector3 velo = dir * m_moveSpeed;

                if (m_isMotionPlay)
                {
                    m_rigidBody.velocity = Vector3.zero;
                }
                else
                {
                    velo.y = m_rigidBody.velocity.y;
                    m_rigidBody.velocity = velo;
                }
            }
        }
    }

    /// <summary>ジャンプ</summary>
    void Jump()
    {
        if (m_isGrounded && m_jump.triggered)
        {
            m_rigidBody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }
    #endregion

    #region Extend Editor 
#if UNITY_EDITOR
    private void OnValidate()
    {

    }

    private void OnDrawGizmos()
    {

    }
#endif
    #endregion
}