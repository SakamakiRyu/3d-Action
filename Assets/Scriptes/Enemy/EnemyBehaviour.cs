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
        None
    }
    #endregion

    #region Serialize Field
    [SerializeField]
    private PlayerBehaviour _Player;

    [SerializeField]
    private Animator _Animator;

    [SerializeField]
    private NavMeshAgent _NavAgent;

    [SerializeField]
    private float _DistanceToBeginAChase = default;

    [SerializeField]
    private int _MaxHP = default;

    [SerializeField]
    private Collider _AttackCollider = default;
    #endregion

    #region Property
    /// <summary>現在のHPを取得</summary>
    public int GetCurrentHP => _CurrentHP;
    /// <summary>現在のステートを取得</summary>
    public State GetCurrentState => _CurrentState;
    #endregion

    #region Private Field
    /// <summary>現在のHP</summary>
    private int _CurrentHP = default;
    /// <summary>プレイヤーとの距離</summary>
    private float _DistanceWithThePlayer = default;
    /// <summary>現在のステート</summary>
    private State _CurrentState;
    #endregion

    #region Unity Function
    private void Awake()
    {
        _CurrentHP = _MaxHP;
    }

    private void Start()
    {
    }

    private void Update()
    {

        _DistanceWithThePlayer = Vector3.Distance(this.transform.position, _Player.transform.position);
        // 死んでいたら何もしない

        // ナビメッシュに目的地を設定する
        if (_DistanceWithThePlayer < _DistanceToBeginAChase)
        {
            return;
        }
    }

    private void LateUpdate()
    {
    }
    #endregion

    #region Public Function
    public void AddDamage(int damage)
    {
        var after = _CurrentHP - damage;
        // Hpが0以下になった時の処理


        _CurrentHP = after;
    }
    #endregion

    #region Private Function
    #endregion
}