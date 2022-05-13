using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystemの入力でHumanoidModelを動かすコンポーネント
/// 移動はカメラを基準にの相対的な移動をする。(常にカメラ前方が正面になる)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour, IDamageable
{
    #region Define
    private PlayerMoveController() { }

    public enum ActionState
    {
        None,
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        Die,
        damaged
    }
    #endregion

    #region Serialize Field
    [SerializeField]
    private PlayerInput _input = default;

    [SerializeField]
    private PlayerParameter _parameter = default;

    [SerializeField]
    private Rigidbody _rigidBody = default;

    [SerializeField]
    private Animator _animator = default;

    [SerializeField]
    private Transform _groundCheckTrasnform = default;

    [SerializeField]
    private Collider _attackCollider = default;

    [SerializeField]
    private TrailRenderer _trail = default;

    [SerializeField]
    private float _moveSpeed = default;

    [SerializeField]
    private float _turnSpeed = default;

    [SerializeField]
    private float _jumpPower = default;
    #endregion

    #region Private Field
    private ActionState _currentActionState = ActionState.Idle;

    /// <summary>接地判定</summary>
    private bool _isGrounded = false;

    // アニメーターのハッシュ
    readonly int _hashDamaged = Animator.StringToHash("Damaged");
    // インプットシステムの入力の取得
    private InputAction _move, _attack, _jump;

    private Vector2 _v2;
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
        var v3 = _rigidBody.velocity;
        v3.y = 0;
        _animator.SetFloat("Speed", v3.magnitude);
        _animator.SetFloat("AnimationSpeed", _v2.magnitude);
        _animator.SetBool("IsGrounded", _isGrounded);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isGrounded) return;

        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            _isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isGrounded) return;

        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
        {
            _isGrounded = false;
        }
    }
    #endregion

    #region Public Function
    public void AddDamage()
    {
        // 既に死んでいたら処理をしない。
        if (_parameter.CurrentState == PlayerParameter.State.Death) return;

        _parameter.ReduceHP();

        if (_parameter.CurrentState == PlayerParameter.State.Death)
        {
            _animator.SetTrigger("Die");
            _rigidBody.isKinematic = true;
        }

        _animator.Play(_hashDamaged);
    }
    #endregion

    #region Private Function
    /// <summary>
    /// インプットシステムの入力を紐づける
    /// </summary>
    private void SetInput()
    {
        _move = _input.currentActionMap["Move"];
        _jump = _input.currentActionMap["Jump"];
        _attack = _input.currentActionMap["Attack1"];
    }

    /// <summary>
    /// AnimationEvent用関数
    /// 剣の軌跡表示の設定
    /// </summary>
    private void TrailSetting(Define.Boolean next)
    {
        if (next == Define.Boolean.False)
        {
            _trail.enabled = false;
        }
        else
        {
            _trail.enabled = true;
        }
    }

    /// <summary>
    /// AnimationEvent用関数
    /// 攻撃に使用するコライダーの設定
    /// </summary>
    private void AttackColliderSetting(Define.Boolean next)
    {
        if (next == Define.Boolean.True)
        {
            _attackCollider.enabled = true;
        }
        else
        {
            _attackCollider.enabled = false;
        }
    }

    /// <summary>
    /// AnimationEvent用関数
    /// </summary>
    private void PlaySound(SoundManager.SEType type)
    {
        SoundManager.Instance.PlaySE(type);
    }

    /// <summary>
    /// イベントに登録
    /// </summary>
    private void Subscribe()
    {

    }

    /// <summary>
    /// イベントの登録解除
    /// </summary>
    private void Unsubscribe()
    {

    }

    /// <summary>
    /// アクションステートの変更をする
    /// </summary>
    private void ChengeActionState(ActionState next)
    {
        switch (next)
        {
            case ActionState.Idle:
                break;
            case ActionState.Walk:
                break;
            case ActionState.Run:
                break;
            case ActionState.Jump:
                break;
            case ActionState.Attack:
                break;
            case ActionState.Die:
                break;
            case ActionState.damaged:
                break;
        }
        _currentActionState = next;
    }

    /// <summary>
    /// ステート毎に毎フレーム呼ばれる処理
    /// </summary>
    private void StateUpdate()
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

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        if (_attack.triggered)
        {
            _animator.SetTrigger("Attack1");
        }
    }

    /// <summary>
    /// 歩く
    /// </summary>
    private void Walk()
    {
        _v2 = _move.ReadValue<Vector2>();
        Vector3 dir = Vector3.forward * _v2.y + Vector3.right * _v2.x;

        if (dir == Vector3.zero)
        {
            if (_isGrounded)
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
            if (_isGrounded)
            {
                //  カメラを基準に移動する
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                //  入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);

                Vector3 velo = dir * _moveSpeed;

                var actState = _currentActionState;

                // 任意のアクションステート時は止まる
                if (actState == ActionState.Attack || actState == ActionState.damaged)
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

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump()
    {
        if (_isGrounded && _jump.triggered)
        {
            _rigidBody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }
    #endregion
}