using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player�̐�����s���N���X
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour, IDamageable
{
    #region Define
    public enum State
    {
        None,
        InGame,
        Death
    }
    #endregion

    #region Serialize Field
    [SerializeField]
    private PlayerInput _Input;

    [SerializeField]
    private Animator _Animator;

    [SerializeField]
    private Rigidbody _Rigidbody;

    [SerializeField]
    private float _MoveSpeed;

    [SerializeField]
    private float _JumpPower;

    [SerializeField]
    private float _TurnSpeed;

    [SerializeField]
    private Transform _LineStart;

    [SerializeField]
    private float _LineLength;

    [SerializeField]
    private int _MaxHP;
    #endregion

    #region Private Field
    /// <summary>���݂̃X�e�[�g</summary>
    private State _CurrentState;
    /// <summary>���݂�HP</summary>
    private int _CurrentHP;
    #endregion

    #region Property
    /// <summary>���݂̃X�e�[�g���擾</summary>
    public State GetCurrentState => _CurrentState;
    /// <summary>���݂�HP���擾</summary>
    public int GetCurrentHP => _CurrentHP;
    #endregion

    #region Input Action
    private InputAction _Move, _Jump;
    #endregion

    #region Unity Function
    private void Awake()
    {
        GetInputActions();
        ChengeState(State.None);
    }

    private void Start()
    {
        ChengeState(State.InGame);
    }

    private void Update()
    {
        UpdateState();
    }

    private void LateUpdate()
    {
        SendToParametarsForAnimator();
    }
    #endregion

    #region Public Function
    public void AddDamage(int damage)
    {
        var after = _CurrentHP - damage;

        if (after <= 0) { ChengeState(State.Death); }

        _CurrentHP = after;
    }
    #endregion

    #region Private Function
    /// <summary>
    /// �X�e�[�g�̕ύX������
    /// </summary>
    /// <param name="next"></param>
    private void ChengeState(State next)
    {
        var prev = _CurrentState;

        // �X�e�[�g�̕ύX���ɂ���������
        switch (next)
        {
            case State.None:
                { }
                break;
            case State.InGame:
                { }
                break;
            case State.Death:
                { }
                break;
        }

        _CurrentState = next;
    }

    /// <summary>
    /// �X�e�[�g���ɖ��t���[���Ă΂�鏈��
    /// </summary>
    private void UpdateState()
    {
        switch (_CurrentState)
        {
            case State.None:
                { }
                break;
            case State.InGame:
                {
                    Move();
                    Jump();
                }
                break;
            case State.Death:
                { }
                break;
        }
    }

    /// <summary>
    /// �ړ�������
    /// </summary>
    private void Move()
    {
        if (_Rigidbody is null) return;

        // �ړ��̓��͂��擾
        var v2 = _Move.ReadValue<Vector2>();
        // �ړ��̕����x�N�g���𐶐�
        var dir = Vector3.right * v2.x + Vector3.forward * v2.y;
        // �n�ʂƂ̐ڒn������擾
        var checkGround = CheckGround();
        // ���x�x�N�g�����擾
        var velo = _Rigidbody.velocity;

        if (dir == Vector3.zero)
        {
            if (checkGround)
            {
                // ���͂������A���n�ʂƐڒn���Ă��鎞��y���W�����ێ�����B(���̏�ł̃W�����v��z��)
                velo.x = 0f; velo.z = 0f;
                _Rigidbody.velocity = velo;
            }
        }
        else
        {
            if (checkGround)
            {
                // �ړ��̓��͂��J��������ɕ␳����
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                // ���͕�����Object�̐��ʂ����炩�ɉ�]������
                var targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _TurnSpeed);

                // ���x�x�N�g���𐶐����ݒ�
                velo = dir.normalized * _MoveSpeed;
                velo.y = _Rigidbody.velocity.y;
                _Rigidbody.velocity = velo;
            }
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump()
    {
        if (_Rigidbody is null) return;

        if (_Jump.triggered)
        {
            // ���x�x�N�g�����擾
            var velo = _Rigidbody.velocity;
            // y�������̃x�N�g����ύX(�W�����v)
            velo.y = _JumpPower;
            // �x�N�g���̐ݒ�
            _Rigidbody.velocity = velo;
        }
    }

    /// <summary>
    /// �n�ʂƂ̐ڒn���������
    /// </summary>
    /// <returns>���茋��</returns>
    private bool CheckGround()
    {
        // �Փ˔������郌�C���[���w��
        var layer = LayerMask.GetMask("Ground");
        // ����Ɏg��Ray�̎n�_
        var start = _LineStart.position;
        // ����Ɏg��Ray�̏I�_
        var end = start + Vector3.down * _LineLength;
        // layer�Ɏw�肵��Object��Ray���Փ˂�����A�ڒn���Ă���Ƃ݂Ȃ��B(�������ĂȂ��ꍇ��False���Ԃ��Ă���)
        var check = Physics.Linecast(start, end, layer);

        if (check) return true;
        return false;
    }

    /// <summary>
    /// �C���v�b�g�A�N�V�����̓��͂�R�Â���
    /// </summary>
    private void GetInputActions()
    {
        _Move = _Input.currentActionMap["Move"];
        _Jump = _Input.currentActionMap["Jump"];
    }

    /// <summary>
    /// �A�j���[�^�[�̍s������Ɏg�����𑗂�
    /// </summary>
    private void SendToParametarsForAnimator()
    {
        if (_Animator is null) return;
        if (_Rigidbody is null) return;

        // �ړ����x(y���̑��x�͖�������)
        var moveSpeed = _Rigidbody.velocity;
        moveSpeed.y = 0;
        _Animator.SetFloat("MoveSpeed", moveSpeed.magnitude);

        // �ڒn���Ă��邩�̏��
        var check = CheckGround();
        _Animator.SetBool("IsGrounded", check);
    }
    #endregion

    #region Callback Function
    #endregion
}
