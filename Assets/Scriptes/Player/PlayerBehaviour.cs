using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Playerの制御を行うクラス
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
    /// ステート毎に毎フレーム呼ばれる処理
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
    /// 移動をする
    /// </summary>
    private void Move()
    {
        if (_Rigidbody is null) return;

        // 移動の入力を取得
        var v2 = _Move.ReadValue<Vector2>();
        // 移動の方向ベクトルを生成
        var dir = Vector3.right * v2.x + Vector3.forward * v2.y;
        // 地面との接地判定を取得
        var checkGround = CheckGround();
        // 速度ベクトルを取得
        var velo = _Rigidbody.velocity;

        if (checkGround)
        {
            if (dir == Vector3.zero)
            {
                // 入力が無い、かつ地面と接地している時はy座標だけ保持する。(その場でのジャンプを想定)
                velo.x = 0f; velo.z = 0f;
                _Rigidbody.velocity = velo;
            }
            else
            {
                // 移動の入力をカメラを基準に補正する
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                // 入力方向にObjectの正面を滑らかに回転させる
                var targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _TurnSpeed);

                // 速度ベクトルを生成し設定
                velo = dir.normalized * _MoveSpeed;
                velo.y = _Rigidbody.velocity.y;
                _Rigidbody.velocity = velo;
            }
        }
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    private void Jump()
    {
        if (_Rigidbody is null) return;

        if (_Jump.triggered)
        {
            // 速度ベクトルを取得
            var velo = _Rigidbody.velocity;
            // y軸方向のベクトルを変更(ジャンプ)
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
        // 衝突判定を取るレイヤーを指定
        var layer = LayerMask.GetMask("Ground");
        // 判定に使うRayの始点
        var start = _LineStart.position;
        // 判定に使うRayの終点
        var end = start + Vector3.down * _LineLength;
        // layerに指定したObjectとRayが衝突したら、接地しているとみなす。(当たってない場合はFalseが返ってくる)
        var check = Physics.Linecast(start, end, layer);

        if (check) return true;
        return false;
    }

    /// <summary>
    /// インプットアクションの入力を紐づける
    /// </summary>
    private void GetInputActions()
    {
        _Move = _Input.currentActionMap["Move"];
        _Jump = _Input.currentActionMap["Jump"];
    }

    /// <summary>
    /// アニメーターの行動制御に使う情報を送る
    /// </summary>
    private void SendToParametarsForAnimator()
    {
        if (_Rigidbody is null) return;
        if (_AnimationControl is null) return;

        // 移動速度(y軸の速度は無視する)
        var moveSpeed = _Rigidbody.velocity;
        moveSpeed.y = 0;
        var sendParam = moveSpeed.magnitude;
        _AnimationControl.SetParameter("MoveSpeed", sendParam);

        // 接地しているかの情報
        var check = CheckGround();
        _AnimationControl.SetParameter("IsGrounded", check);
    }
    #endregion

    #region Callback Function
    #endregion
}
