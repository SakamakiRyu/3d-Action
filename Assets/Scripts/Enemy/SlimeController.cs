using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵クラス。経路探索はNavmeshで行う
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SlimeController : MonoBehaviour, IDamageable
{
    #region Define
    public enum ActionState
    {
        None,
        Idle,
        Run,
        Attack,
        Dizzy,
        Die
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
    private float _startChaseDistance = default;

    [SerializeField]
    private int _maxHP = default;

    [SerializeField]
    private Collider _attackCollider = default;

    [SerializeField]
    private HPUIController _HPUIController = default;

    [SerializeField]
    private MissionControl _mission = default;

    public bool IsGameEnd { get; private set; } = false;

    public bool IsDead => _currentHP <= 0;

    private ActionState _currentState = ActionState.None;

    private int _currentHP = default;
    /// <summary>HPゲージの表示割合を返す</summary>
    public float GetUIValue => (float)_currentHP / _maxHP;
    /// <summary>Playerとの距離を格納する変数</summary>
    private float _distance = default;

    // アニメーションのハッシュ
    readonly int _hashDizzy = Animator.StringToHash("Dizzy");
    readonly int _hashDie = Animator.StringToHash("Die");
    #endregion

    #region Unity Function
    private void Awake()
    {
        _currentHP = _maxHP;
        _currentState = ActionState.Idle;
    }

    private void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        SendParameterForAnimator();
    }
    #endregion

    private enum MovingState { Chaseing, Waiting }

    /// <summary>
    /// AnimationEvent用関数
    /// ステートの切り替え
    /// </summary>
    private void ChengeState(MovingState state)
    {
        if (state == MovingState.Chaseing)
        {
            _nav.SetDestination(_player.transform.position);
            _nav.isStopped = true;
        }
        else
        {
            _nav.SetDestination(this.transform.position);
            _nav.isStopped = false;
        }
    }

    private void ChengeActionState(ActionState next)
    {
                    
    }

    public void AddDamage()
    {
        if (IsDead) return;

        _currentHP--;
        _HPUIController.UpdateHPSlider(this);

        var check = AriveCheck();

        if (!check)
        {
            _animator.Play(_hashDie);
            _mission.AddScore();
            return;
        }

        EffectManager.Instance.PlayEffect(EffectManager.EffectType.HitStop);
        _animator.Play(_hashDizzy);
    }

    private void Move()
    {
        if (IsGameEnd) return;

        _distance = Vector3.Distance(this.transform.position, _player.transform.position);

        if (IsDead) return;

        if (_distance < _startChaseDistance)
        {
            _nav.SetDestination(_player.transform.position);
            return;
        }
        _nav.SetDestination(this.transform.position);
    }

    /// <summary>
    /// アニメーターに任意のパラメータを渡す
    /// </summary>
    private void SendParameterForAnimator()
    {
        if (IsGameEnd)
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
    /// 音を鳴らす
    /// </summary>
    private void PlaySound(SoundManager.SEType type)
    {
        SoundManager.Instance.PlaySE(type);
    }

    /// <summary>
    /// AnimationEvent用関数
    /// 攻撃に使用するコライダーの設定
    /// </summary>
    /// <param name="next"></param>
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

    /// <summary>
    /// 生存しているか
    /// true = 生存
    /// </summary>
    private bool AriveCheck()
    {
        return _currentHP > 0;
    }
}