using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystem�̓��͂�HumanoidModel�𓮂����R���|�[�l���g
/// �ړ��̓J��������ɂ̑��ΓI�Ȉړ�������B(��ɃJ�����O�������ʂɂȂ�)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    #region Define
    public enum State
    {
        None,
        Move,
        Death
    }
    #endregion

    #region Serialize Field
    [SerializeField]
    private PlayerInput _Input;

    [SerializeField]
    private Rigidbody _Rigidbody;

    [SerializeField]
    private Animator _Animator;

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
    /// <summary>���݂̃X�e�[�g</summary>
    private State _CurrentState;
    #endregion

    #region Property
    /// <summary>���݂̃X�e�[�g���擾</summary>
    public State GetCurrentState => _CurrentState;
    #endregion

    #region Input Action
    private InputAction _Move, _Jump;
    #endregion

    #region Unity Function
    private void Awake()
    {
        ChengeState(State.None);
    }

    private void Start()
    {
        GetInputActions();
        ChengeState(State.Move);
    }

    private void Update()
    {
        StateUpdate();
    }
    #endregion

    #region Public Function
    #endregion

    #region Private Function
    /// <summary>
    /// �X�e�[�g�̕ύX������
    /// </summary>
    /// <param name="next"></param>
    private void ChengeState(State next)
    {
        var prev = _CurrentState;

        switch (next)
        {
            case State.None:
                { }
                break;
            case State.Move:
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
    private void StateUpdate()
    {
        switch (_CurrentState)
        {
            case State.None:
                { }
                break;
            case State.Move:
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
        if (!_Rigidbody) return;

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
                // ���͂��J��������ɕ␳����
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                // ���͕�����Object�̐��ʂ����炩�ɉ�]������
                var targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, _TurnSpeed);

                // ���x�x�N�g���𐶐����ݒ�
                velo = dir * _MoveSpeed;
                _Rigidbody.velocity = velo;
            }
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump()
    {
        if (!_Rigidbody) return;

        if (_Jump.triggered)
        {
            // ���x�x�N�g�����擾
            var velo = _Rigidbody.velocity;
            // y�����̃x�N�g����ύX(�W�����v)
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
        var layer = LayerMask.GetMask("Ground");

        var check = Physics.Raycast(this.transform.position, Vector3.down, _LineLength, layer);

        if (check) return true;
        return false;
    }

    /// <summary>
    /// �C���v�b�g�A�N�V�����ł̓��͂��擾
    /// </summary>
    private void GetInputActions()
    {
        _Move = _Input.currentActionMap["Move"];
        _Jump = _Input.currentActionMap["Jump"];
    }
    #endregion

    #region Callback Function
    #endregion
}
