using UnityEngine;
using UnityEngine.AI;

// 短気制作かつ、個人製作の為オブジェクト指向は考慮していない
public class EnemyController : MonoBehaviour, IGameClear
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

    public bool IsDead => m_currentHP < 1;

    int m_currentHP = default;
    float m_distance = default;

    readonly int m_hashDizzy = Animator.StringToHash("Dizzy");
    readonly int m_hashDie = Animator.StringToHash("Die");

    Animator m_animator;
    NavMeshAgent m_nav;

    private void Awake()
    {
        m_currentHP = m_maxHP;
    }

    private void Start()
    {
        Register();
        m_animator = GetComponent<Animator>();
        m_nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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

    public void GetDamage()
    {
        m_currentHP--;
        if (IsDead)
        {
            m_animator.Play(m_hashDie);
            return;
        }
        m_animator.Play(m_hashDizzy);
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

    private void OnDestroy()
    {
        QuestManager.Instance.GameClear -= OnClear;
        QuestManager.Instance.GameUpdate();
    }

    public void Register()
    {
        QuestManager.Instance.GameClear += OnClear;
    }

    public void OnClear()
    {
        m_nav.isStopped = true;
    }
}