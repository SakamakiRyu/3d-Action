using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusBase : MonoBehaviour
{
    [Header("ステータス")]
    /// <summary>ヒットポイント</summary>
    [SerializeField] protected int m_hp = default;
    /// <summary>攻撃力</summary>
    [SerializeField] protected int m_atkPower = default;

    /// <summary>攻撃メソッド</summary>
    /// <param name="target">攻撃対象</param>
    public virtual void Attack(StatusBase target)
    {
        BattleManager.Instance.DamageCalculator(target, this);
    }

    /// <summary>自身の攻撃力を渡す</summary>
    public virtual int SendMyAttackPower()
    {
        int totalAtkPower = default;
        totalAtkPower += m_atkPower;
        return totalAtkPower;
    }

    /// <summary>ダメージを受ける</summary>
    public virtual void Damaged(int damage)
    {
        m_hp -= damage;
    }
}
