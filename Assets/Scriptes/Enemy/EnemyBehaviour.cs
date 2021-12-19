using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の行動管理クラス。
/// 経路探索はNavmeshで行う。
/// </summary>
public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    #region Define
    public enum State
    {
        None,
        Idle,
        Chasing,
        Attack,
        Death
    }
    #endregion

    #region Serialize Field
    [SerializeField]
    private PlayerBehaviour _Player;

    [SerializeField]
    private EnemyParameter _EnemyParameter;

    [SerializeField]
    private AnimationControl _AnimationControl;

    [SerializeField]
    private NavMeshAgent _NavAgent;

    [SerializeField]
    private Collider _AttackCollider;

    [SerializeField]
    private float _BeginChaseDistance;
    #endregion

    #region Property
    /// <summary>現在のHPを取得</summary>
    public int GetCurrentHP => _CurrentHP;
    /// <summary>現在のステートを取得</summary>
    public State GetCurrentState => _CurrentState;
    #endregion

    #region Private Field
    /// <summary>現在のHP</summary>
    private int _CurrentHP;
    /// <summary>プレイヤーとの距離</summary>
    private float _DistanceThePlayer;
    /// <summary>現在のステート</summary>
    private State _CurrentState;
    #endregion

    #region Unity Function
    private void Awake()
    {
        _CurrentHP = _EnemyParameter.MaxHP;
    }

    private void Start() { }

    private void Update()
    {
        StateUpdate();
    }

    private void LateUpdate() { }

    private void FixedUpdate() { }
    #endregion

    #region Public Function
    public void AddDamage(int damage)
    {
        var after = _CurrentHP - damage;
        // Hpが0以下になった時の処理
        if (after <= 0)
        {
            ChengeState(State.Death);
            return;
        }

        _CurrentHP = after;
    }
    #endregion

    #region Private Function
    /// <summary>
    /// ステートの変更をする
    /// </summary>
    /// <param name="next">次のステート</param>
    private void ChengeState(State next)
    {
        var prev = _CurrentState;

        // ステートの変更時にする処理
        switch (next)
        {
            case State.None:
                { }
                break;
            case State.Idle:
                { }
                break;
            case State.Chasing:
                { }
                break;
            case State.Attack:
                { }
                break;
            case State.Death:
                { }
                break;
        }

        _CurrentState = next;
    }

    private void StateUpdate()
    {
        switch (_CurrentState)
        {
            case State.None:
                { }
                break;
            case State.Idle:
                { }
                break;
            case State.Chasing:
                { }
                break;
            case State.Attack:
                { }
                break;
            case State.Death:
                { }
                break;
        }
    }

    /// <summary>
    /// プレイヤーの追跡をする
    /// </summary>
    private void CheseToPlayer()
    {

    }
    #endregion
}