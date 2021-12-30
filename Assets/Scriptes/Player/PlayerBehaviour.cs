using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player�̐�����s���N���X
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    #region Define
    #endregion

    #region Serialize Field
    [SerializeField]
    private PlayerInput _Input;

    [SerializeField]
    private AnimationControl _AnimationControl;

    [SerializeField]
    private AttackBehaviour _Attack;

    [SerializeField]
    private PlayerParameter _Parameter;

    [SerializeField]
    private Transform _LineStart;

    [SerializeField]
    private Rigidbody _Rigidbody;

    [SerializeField]
    private float _MoveSpeed;

    [SerializeField]
    private float _JumpPower;

    [SerializeField]
    private float _TurnSpeed;

    [SerializeField]
    private float _LineLength;
    #endregion

    #region Private Field
    #endregion

    #region Property
    #endregion

    #region Input Action
    private InputAction _Move, _Jump;
    #endregion

    #region Unity Function
    private void Awake()
    {
        GetInputActions();
    }

    private void Update()
    {
        StateUpdate();
    }

    private void LateUpdate()
    {
        SendToParametarsForAnimator();
    }
    #endregion

    #region Public Function
    #endregion

    #region Private Function
    /// <summary>
    /// �X�e�[�g���ɖ��t���[���Ă΂�鏈��
    /// </summary>
    private void StateUpdate()
    {
        switch (_Parameter.GetCurrentState)
        {
            case PlayerParameter.State.None:
                { }
                break;
            case PlayerParameter.State.Arive:
                {
                    Move();
                    Jump();
                }
                break;
            case PlayerParameter.State.Death:
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

        if (checkGround)
        {
            if (dir == Vector3.zero)
            {
                // ���͂������A���n�ʂƐڒn���Ă��鎞��y���W�����ێ�����B(���̏�ł̃W�����v��z��)
                velo.x = 0f; velo.z = 0f;
                _Rigidbody.velocity = velo;
            }
            else
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
        if (_Rigidbody is null) return;
        if (_AnimationControl is null) return;

        // �ړ����x(y���̑��x�͖�������)
        var moveSpeed = _Rigidbody.velocity;
        moveSpeed.y = 0;
        var sendParam = moveSpeed.magnitude;
        _AnimationControl.SetParameter("MoveSpeed", sendParam);

        // �ڒn���Ă��邩�̏��
        var check = CheckGround();
        _AnimationControl.SetParameter("IsGrounded", check);
    }
    #endregion

    #region Callback Function
    #endregion
}
