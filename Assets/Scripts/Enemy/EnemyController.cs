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

    [SerializeField]
    NavMeshAgent m_nav = default;

    [SerializeField]
    Transform m_targetTransform = default;

    [SerializeField]
    float m_startChaseDistance = default;

    [SerializeField]
    int m_maxHP = default;

    [SerializeField]
    Collider m_attackCollider = default;

    [SerializeField]
    HPUIController m_HPUIController = default;

    public bool IsGameEnd { get; private set; } = false;
    public bool IsDead => m_currentHP < 1;

    int m_currentHP = default;
    /// <summary>HPゲージの表示割合を返す</summary>
    public float GetUIValue => (float)m_currentHP / (float)m_maxHP;
    /// <summary>Playerとの距離を格納する変数</summary>
    float m_distance = default;

    // アニメーションのハッシュ
    readonly int m_hashDizzy = Animator.StringToHash("Dizzy");
    readonly int m_hashDie = Animator.StringToHash("Die");

    AudioSource m_source;
    Animator m_animator;

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

    // AnimationEvent用のフラグ
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
        m_HPUIController.UpdateHPSlider(this);

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
        Mission.Instance.OnGameEnd += OnEnd;
    }

    private void OnDestroy()
    {
        Mission.Instance.OnGameEnd -= OnEnd;
        Mission.Instance.GameScoreUp();
    }

    float _Timer = 0f;

    IEnumerator HitStopAcync()
    {
        _Timer = 0f;
        Time.timeScale = 0.1f;
        while (_Timer < 0.4f)
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