using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

/// <summary>敵クラス</summary>
public class Enemy : CharactorBase
{
    [SerializeField, Header("　　　　能力値パラメータ")]
    Breed m_breed = null;

    [SerializeField, Header("追跡をはじめる距離")]
    float m_beginChanseDistance = default;

    [SerializeField, Header("追跡をやめる距離")]
    float m_stopDistance = default;

    [SerializeField, Header("追跡対象")]
    GameObject m_target = default;

    float m_distance;
    public Sprite Sprite => m_breed.Sprite;
    public int MaxHP => m_breed.MaxHp;
    Transform m_targetTransform = default;
    NavMeshAgent m_navMesh;
    Animator m_anim;

    private void Start()
    {
        CurrentHP = MaxHP;
        m_navMesh = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_navMesh.stoppingDistance = m_stopDistance;
        m_navMesh.speed = m_moveSpeed;
    }

    private void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        m_anim.SetFloat("Speed", m_navMesh.velocity.magnitude);
        m_anim.SetFloat("Distance", m_distance);
    }

    public override void Move()
    {
        m_targetTransform = m_target.transform;
        m_distance = Vector3.Distance(this.gameObject.transform.position, m_targetTransform.position);
        if (m_distance < m_beginChanseDistance)
        {
            this.transform.LookAt(m_target.transform);
            if (m_distance > m_stopDistance)
            {
                m_navMesh.SetDestination(m_targetTransform.position);
            }
        }
        else
        {
            m_navMesh.SetDestination(this.transform.position);
        }
    }

    public enum IsAttack { True, False }

    public void SetDestination(IsAttack isAttack)
    {
        if (isAttack.Equals(IsAttack.True))
        {
            m_navMesh.SetDestination(this.transform.position);
        }
        else
        {
            m_navMesh.SetDestination(m_targetTransform.position);
        }
    }

    public override int SendAtkPower()
    {
        return m_breed.AtkPower;
    }
}