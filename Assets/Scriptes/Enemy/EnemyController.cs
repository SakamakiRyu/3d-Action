using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// 敵クラス。経路探索はNavmeshで行う
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour, IGameEnd, IDamageable
{
    EnemyController() { }
    /// <summary>追跡対象</summary>
    [SerializeField] Transform m_targetTransform = default;
    /// <summary>追跡をはじめる距離</summary>
    [SerializeField] float m_startChaseDistance = default;
    /// <summary>最大HP</summary>
    [SerializeField] int m_maxHP = default;
    /// <summary>攻撃の当たり判定</summary>
    [SerializeField] Collider m_attackCollider = default;

    public bool IsGameEnd { get; private set; } = false;
    public bool IsDead => m_currentHP < 1;

    int m_currentHP = default;
    float m_distance = default;

    readonly int m_hashDizzy = Animator.StringToHash("Dizzy");
    readonly int m_hashDie = Animator.StringToHash("Die");

    AudioSource m_source;
    Animator m_animator;
    NavMeshAgent m_nav;

    private void Awake()
    {
        m_currentHP = m_maxHP;
    }

    private void Start()
    {
        Register();
        m_source = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        m_nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (IsGameEnd) return;

        m_distance = Vector3.Distance(this.transform.position, m_targetTransform.position);
        // 死んでいたら何もしない
        if (IsDead) return;

        // ナビメッシュに目的地を設定する
        if (m_distance < m_startChaseDistance)
        {
            m_nav.SetDestination(m_targetTransform.position);
            return;
        }
        m_nav.SetDestination(this.transform.position);
    }

    private void LateUpdate()
    {
        if (IsGameEnd)
        {
            m_animator.SetFloat("Distance", 100);
            return;
        }
        m_animator.SetFloat("Speed", m_nav.velocity.magnitude);
        m_animator.SetFloat("Distance", m_distance);
    }

    public enum IsStop { True, False }
    public void OnChengeState(IsStop isStop)
    {
        if (isStop == IsStop.True)
        {
            m_nav.isStopped = true;
        }
        else
        {
            m_nav.isStopped = false;
        }
    }

    public void AddDamage(int damage)
    {
        m_currentHP--;

        if (IsDead)
        {
            m_animator.Play(m_hashDie);
            return;
        }
        StartCoroutine(HitStopAcync());
        m_animator.Play(m_hashDizzy);
    }

    public void OnPlaySound(AudioClip clip)
    {
        m_source.PlayOneShot(clip);
    }

    public void BeginAttack()
    {
        m_attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        m_attackCollider.enabled = false;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void Register()
    {
        QuestManager.Instance.GameEnd += OnEnd;
    }

    private void OnDestroy()
    {
        QuestManager.Instance.GameEnd -= OnEnd;
        QuestManager.Instance.GameUpdate();
    }

    float _Timer = 0f;

    IEnumerator HitStopAcync()
    {
        _Timer = 0f;
        Time.timeScale = 0;
        while (_Timer < 0.2f)
        {
            _Timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1.0f;
        yield return null;
    }

    public void OnEnd()
    {
        m_nav.isStopped = true;
        IsGameEnd = true;
    }
}