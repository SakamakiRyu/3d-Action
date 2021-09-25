using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>InputSystemの入力でHumanoidModelを動かすコンポーネント</summary>
/// 移動はカメラを基準にの相対的な移動をする。(常にカメラ前方が正面になる)
[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    PlayerController() { }

    #region メンバー変数

    [SerializeField] PlayerInput m_pInput = default;
    [SerializeField] Transform m_isGroundedTrasnform = default;
    [SerializeField] Collider m_attackCollider = default;
    [SerializeField] TrailRenderer m_trail = default;
    [SerializeField] float m_moveSpeed = default;
    [SerializeField] float m_turnSpeed = default;
    [SerializeField] float m_jumpPower = default;

    bool IsGrounded = default;

    InputAction m_move, m_attack, m_jump;
    Rigidbody m_rb;
    Animator m_anim;
    AudioSource m_source;
    Vector2 m_v2;

    #endregion 

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_source = GetComponent<AudioSource>();
        SetInput();
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
        m_anim.SetFloat("Speed", v3.magnitude);
        m_anim.SetFloat("AnimationSpeed", m_v2.magnitude);
        m_anim.SetBool("IsGrounded", IsGrounded);
    }

    void Attack()
    {
        if (m_attack.triggered)
        {
            m_anim.SetTrigger("Attack1");
        }
    }

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

    public void OnPlaySound(AudioClip clip)
    {
        m_source.PlayOneShot(clip);
    }

    /// <summary>インプットシステムの入力を割り当てる</summary>
    void SetInput()
    {
        m_move = m_pInput.currentActionMap["Move"];
        m_jump = m_pInput.currentActionMap["Jump"];
        m_attack = m_pInput.currentActionMap["Attack1"];
    }

    /// <summary>Playerの移動</summary>
    void Move()
    {
        m_v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * m_v2.y + Vector3.right * m_v2.x;

        if (dir == Vector3.zero)
        {
            if (IsGrounded)
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
            // 地面と接地していれば移動可能
            if (IsGrounded)
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

    /// <summary>ジャンプ</summary>
    void Jump()
    {
        if (IsGrounded && m_jump.triggered)
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            IsGrounded = true;
            Debug.Log($"{other.name}と接地");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            IsGrounded = false;
            Debug.Log("離地");
        }
    }

    public void BeginAttack()
    {
        m_attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        m_attackCollider.enabled = false;
    }

    public void GetDamage()
    {

    }

#if UNITY_EDITOR
    private void OnValidate()
    {

    }

    private void OnDrawGizmos()
    {

    }
#endif
}