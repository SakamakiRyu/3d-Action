using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystemの入力でHumanoidModelを動かすコンポーネント
/// 移動はカメラを基準にの相対的な移動をする。(常にカメラ前方が正面になる)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour, IDamageable
{
    enum MoveState
    {
        Idle,
        Run,
        Attack
    }

    #region Serialize Field
    [SerializeField]
    PlayerInput m_input = default;

    [SerializeField]
    PlayerParameter _parameter = default;

    [SerializeField]
    Rigidbody _rigidBody = default;

    [SerializeField]
    Animator _animator = default;

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
    bool m_isGrounded = false;
    bool m_isGameEnded = false;
    bool m_isMotionPlay = false;

    // アニメーターのハッシュ
    readonly int m_hashDamaged = Animator.StringToHash("Damaged");
    // インプットシステムの入力の取得
    InputAction _move, _attack, m_jump;

    Vector2 _v2;
    #endregion

    #region Unity Function
    private void Start()
    {
        SetInput();
    }

    private void Update()
    {
        StateUpdate();
    }

    private void LateUpdate()
    {
        Vector3 v3 = _rigidBody.velocity;
        v3.y = 0;
        _animator.SetFloat("Speed", v3.magnitude);
        _animator.SetFloat("AnimationSpeed", _v2.magnitude);
        _animator.SetBool("IsGrounded", m_isGrounded);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            m_isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
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
    /// <summary>
    /// 剣の軌跡を表示を操作する
    /// </summary>
    public void TrailSetting(IsVisible isVisible)
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

    public void AddDamage()
    {
        // 既に死んでいたら処理をしない。
        if (_parameter.CurrentState == PlayerParameter.State.Death) return;

        _parameter.ReduceHP();

        // StateCheck();
        if (_parameter.CurrentState == PlayerParameter.State.Death)
        {
            _animator.SetTrigger("Die");
            _rigidBody.isKinematic = true;
        }

        _animator.Play(m_hashDamaged);
    }

    /// <summary>AnimationEvent用関数</summary>
    public void OnEnd()
    {
        m_isGameEnded = true;
    }

    /// <summary>AnimationEvent用関数</summary>
    /// <param name="clip"></param>
    public void PlayFootstep(AudioClip clip)
    {
        SoundManager.Instance.PlaySE(SoundManager.SEType.PlayerFootStep);
    }
    #endregion

    #region Private Function
    /// <summary>インプットシステムの入力を取得</summary>
    private void SetInput()
    {
        _move = m_input.currentActionMap["Move"];
        m_jump = m_input.currentActionMap["Jump"];
        _attack = m_input.currentActionMap["Attack1"];
    }

    /// <summary>イベントに登録</summary>
    private void Subscribe()
    {

    }

    /// <summary>イベントの登録解除</summary>
    private void Unsubscribe()
    {

    }

    /// <summary>ステート毎に毎フレーム呼ばれる処理</summary>
    void StateUpdate()
    {
        switch (_parameter.CurrentState)
        {
            case PlayerParameter.State.None:
                { }
                break;
            case PlayerParameter.State.Arive:
                {
                    Walk();
                    Jump();
                    Attack();
                }
                break;
            case PlayerParameter.State.Death:
                {
                }
                break;
        }
    }

    private void Attack()
    {
        if (_attack.triggered)
        {
            _animator.SetTrigger("Attack1");
        }
    }

    /// <summary>歩く</summary>
    private void Walk()
    {
        _v2 = _move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * _v2.y + Vector3.right * _v2.x;

        if (dir == Vector3.zero)
        {
            if (m_isGrounded)
            {
                _rigidBody.velocity = new Vector3(0f, _rigidBody.velocity.y, 0f);
            }
            else
            {
                _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y, _rigidBody.velocity.z);
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
                    _rigidBody.velocity = Vector3.zero;
                }
                else
                {
                    velo.y = _rigidBody.velocity.y;
                    _rigidBody.velocity = velo;
                }
            }
        }
    }

    /// <summary>ジャンプ</summary>
    private void Jump()
    {
        if (m_isGrounded && m_jump.triggered)
        {
            _rigidBody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }
    }
    #endregion
}