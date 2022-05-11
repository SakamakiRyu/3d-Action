﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// 敵クラス。経路探索はNavmeshで行う
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    private EnemyController() { }

    [SerializeField]
    private NavMeshAgent _nav = default;

    [SerializeField]
    private Animator _animator = default;

    [SerializeField]
    private Transform _targetTransform = default;

    [SerializeField]
    private float _startChaseDistance = default;

    [SerializeField]
    private int _maxHP = default;

    [SerializeField]
    private Collider _attackCollider = default;

    [SerializeField]
    private HPUIController _HPUIController = default;

    [SerializeField]
    private Mission _mission = default;

    public bool IsGameEnd { get; private set; } = false;

    public bool IsDead => _currentHP <= 0;

    private int _currentHP = default;
    /// <summary>HPゲージの表示割合を返す</summary>
    public float GetUIValue => (float)_currentHP / _maxHP;

    /// <summary>Playerとの距離を格納する変数</summary>
    private float _distance = default;

    // アニメーションのハッシュ
    readonly int _hashDizzy = Animator.StringToHash("Dizzy");
    readonly int _hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    private void Start()
    {
        Subscribe();
    }

    private void Update()
    {
        if (IsGameEnd) return;

        _distance = Vector3.Distance(this.transform.position, _targetTransform.position);
        // 死んでいたら何もしない
        if (IsDead) return;

        // ナビメッシュに目的地を設定する
        if (_distance < _startChaseDistance)
        {
            _nav.SetDestination(_targetTransform.position);
            return;
        }
        _nav.SetDestination(this.transform.position);
    }

    private void LateUpdate()
    {
        if (IsGameEnd)
        {
            _animator.SetFloat("Distance", 100);
            return;
        }
        _animator.SetFloat("Speed", _nav.velocity.magnitude);
        _animator.SetFloat("Distance", _distance);
    }

    // AnimationEvent用のフラグ
    public enum MovingState { Chaseing, Waiting }
    public void ChengeState(MovingState isStop)
    {
        if (isStop == MovingState.Chaseing)
        {
            _nav.isStopped = true;
        }
        else
        {
            _nav.isStopped = false;
        }
    }

    public void AddDamage()
    {
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

    public void Subscribe()
    {
        GameManager.Instance.OnGameEnd += MoveStop;
    }

    public void Unsubscribe()
    {
        GameManager.Instance.OnGameEnd -= MoveStop;
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

    

    /// <summary>
    /// navメッシュの追跡をやめる
    /// </summary>
    private void MoveStop()
    {
        Unsubscribe();
        _nav.isStopped = true;
        IsGameEnd = true;
    }
}