using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵クラス。経路探索はNavmeshで行う
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    #region Define
    public enum MovingState
    {
        None,
        Idling,
        Chasing,
        Attacking,
        Dead
    }
    #endregion

    #region Field
    [SerializeField]
    private NavMeshAgent _nav = default;

    [SerializeField]
    private Animator _animator = default;

    [SerializeField]
    private PlayerMoveController _player = default;

    [SerializeField]
    private Collider _attackCollider = default;

    [SerializeField]
    private EnemyUIController _uiController = default;

    [SerializeField]
    private MissionControl _mission = default;

    [Header("追跡を始める距離")]
    [SerializeField]
    private float _chaseStartDistance = default;

    [SerializeField]
    private int _maxHP = default;

    public bool IsGameEnd { get; private set; } = false;
    public bool IsDead => _currentHP <= 0;

    /// <summary>現在のHP</summary>
    private int _currentHP = default;
    /// <summary>Playerとの距離を格納する変数</summary>
    private float _distance = default;

    // アニメーションのハッシュ
    readonly int _hashDizzy = Animator.StringToHash("Dizzy");
    readonly int _hashDie = Animator.StringToHash("Die");

    private MovingState _currentMovingState = MovingState.None;
    #endregion

    #region Unity Function
    private void Awake()
    {
        _currentHP = _maxHP;
    }

    private void Start()
    {
        ChengeMovingState(MovingState.Idling);
    }

    private void Update()
    {
        _distance = Vector3.Distance(this.transform.position, _player.transform.position);
        StateUpdate();
    }

    private void LateUpdate()
    {
        SendParameterForAnimator();
    }
    #endregion

    #region Public Function
    public void AddDamage()
    {
        if (IsDead) return;

        _currentHP--;

        if (_uiController)
        {
            var current = (float)_currentHP;
            var max = (float)_maxHP;
            // HPゲージの更新
            _uiController.UpdateHPGauge(current, max);
        }

        if (_currentHP <= 0)
        {
            _animator.Play(_hashDie);
            _mission.AddScore();
            return;
        }

        EffectManager.Instance.PlayHitStop();
        _animator.Play(_hashDizzy);
    }
    #endregion

    #region Private Fucntion
    /// <summary>
    /// ステート毎に呼ばれる処理
    /// </summary>
    private void StateUpdate()
    {
        switch (_currentMovingState)
        {
            case MovingState.Idling:
                {
                    if (_distance < _chaseStartDistance)
                    {
                        ChengeMovingState(MovingState.Chasing);
                    }
                }
                break;
            case MovingState.Chasing:
                { }
                break;
            case MovingState.Attacking:
                { }
                break;
            case MovingState.Dead:
                { }
                break;
        }
    }

    /// <summary>
    /// ステートの切り替えをする
    /// </summary>
    private void ChengeMovingState(MovingState next)
    {
        switch (next)
        {
            case MovingState.Idling:
                {
                    _nav.SetDestination(this.transform.position);
                }
                break;
            case MovingState.Chasing:
                {
                    _nav.SetDestination(_player.transform.position);
                }
                break;
            case MovingState.Attacking:
                {
                    _nav.SetDestination(this.transform.position);
                }
                break;
            case MovingState.Dead:
                {
                    _nav.SetDestination(this.transform.position);
                }
                break;
        }

        _currentMovingState = next;
    }

    /// <summary>
    /// アニメーターに任意のパラメータを渡す
    /// </summary>
    private void SendParameterForAnimator()
    {
        // カットシーン再生中
        if (_mission.IsPlayedMovie == false)
        {
            _animator.SetFloat("Distance", 100);
            return;
        }
        _animator.SetFloat("Speed", _nav.velocity.magnitude);
        _animator.SetFloat("Distance", _distance);
    }

    /// <summary>
    /// Animation用の関数
    /// </summary>
    private void Destroy()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// AnimationEvent用関数
    /// <para>音を鳴らす</para>
    /// </summary>
    private void PlaySound(SoundManager.SEType type)
    {
        SoundManager.Instance.PlaySE(type);
    }

    /// <summary>
    /// AnimationEvent用関数
    /// <para>攻撃に使用するコライダーの設定</para>
    /// </summary>
    private void AttackColliderSetting(Define.Boolean next)
    {
        switch (next)
        {
            case Define.Boolean.True:
                {
                    _attackCollider.enabled = true;
                }
                break;
            case Define.Boolean.False:
                {
                    _attackCollider.enabled = false;
                }
                break;
        }
    }
    #endregion
}