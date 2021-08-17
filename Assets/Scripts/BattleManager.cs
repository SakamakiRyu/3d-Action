using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager m_instance;
    public static BattleManager Instance => m_instance;
    private BattleManager() { }

    private void Awake()
    {
        m_instance = this;
    }

    public void Battle(StatusBase target, StatusBase attacker)
    {
        DamageCalculator(target, attacker);
    }

    /// <summary>
    /// ダメージ計算をし、ダメージ結果を攻撃対象に渡す
    /// </summary>
    /// <param name="target">攻撃対象</param>
    /// <param name="attacker">攻撃者</param>
    public void DamageCalculator(StatusBase target, StatusBase attacker)
    {
        int totalDamage = 0;
        totalDamage += attacker.SendMyAttackPower();
        target.Damaged(totalDamage);
    }
}
