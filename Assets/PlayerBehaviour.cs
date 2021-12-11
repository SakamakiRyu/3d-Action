using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystemの入力でHumanoidModelを動かすコンポーネント
/// 移動はカメラを基準にの相対的な移動をする。(常にカメラ前方が正面になる)
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
    /// <summary>現在のステート</summary>
    private State _CurrentState;
    #endregion

    #region Property
    /// <summary>現在のステートを取得</summary>
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
    /// ステートの変更をする
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
    /// ステート毎に毎フレーム呼ばれる処理
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
    /// 移動をする
    /// </summary>
    private void Move()
    {
        if (!_Rigidbody) return;

        // 移動の入力を取得
        var v2 = _Move.ReadValue<Vector2>();
        // 移動の方向ベクトルを生成
        var dir = Vector3.right * v2.x + Vector3.forward * v2.y;
        // 地面との接地判定を取得
        var checkGround = CheckGround();
        // 速度ベクトルを取得
        var velo = _Rigidbody.velocity;

        if (dir == Vector3.zero)
        {
            if (checkGround)
            {
                // 入力が無い、かつ地面と接地している時はy座標だけ保持する。(その場でのジャンプを想定)
                velo.x = 0f; velo.z = 0f;
                _Rigidbody.velocity = velo;
            }
        }
        else
        {
            if (checkGround)
            {
                // 入力をカメラを基準に補正する
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                // 入力方向にObjectの正面を滑らかに回転させる
                var targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, _TurnSpeed);

                // 速度ベクトルを生成し設定
                velo = dir * _MoveSpeed;
                _Rigidbody.velocity = velo;
            }
        }
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    private void Jump()
    {
        if (!_Rigidbody) return;

        if (_Jump.triggered)
        {
            // 速度ベクトルを取得
            var velo = _Rigidbody.velocity;
            // y方向のベクトルを変更(ジャンプ)
            velo.y = _JumpPower;
            // ベクトルの設定
            _Rigidbody.velocity = velo;
        }
    }

    /// <summary>
    /// 地面との接地判定をする
    /// </summary>
    /// <returns>判定結果</returns>
    private bool CheckGround()
    {
        var layer = LayerMask.GetMask("Ground");

        var check = Physics.Raycast(this.transform.position, Vector3.down, _LineLength, layer);

        if (check) return true;
        return false;
    }

    /// <summary>
    /// インプットアクションでの入力を取得
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
