using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Playerの制御を行うクラス
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
    /// <summary>現在のステート</summary>
    private State _CurrentState;
    /// <summary>現在のHP</summary>
    private int _CurrentHP;
    #endregion

    #region Property
    /// <summary>現在のステートを取得</summary>
    public State GetCurrentState => _CurrentState;
    /// <summary>現在のHPを取得</summary>
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
    /// ステートの変更をする
    /// </summary>
    /// <param name="next"></param>
    private void ChengeState(State next)
    {
        var prev = _CurrentState;

        // ステートの変更時にしたい処理
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
    /// ステート毎に毎フレーム呼ばれる処理
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
        if (_Animator is null) return;
        if (_Rigidbody is null) return;

        // 移動速度(y軸の速度は無視する)
        var moveSpeed = _Rigidbody.velocity;
        moveSpeed.y = 0;
        _Animator.SetFloat("MoveSpeed", moveSpeed.magnitude);

        // 接地しているかの情報
        var check = CheckGround();
        _Animator.SetBool("IsGrounded", check);
    }
    #endregion

    #region Callback Function
    #endregion
}
