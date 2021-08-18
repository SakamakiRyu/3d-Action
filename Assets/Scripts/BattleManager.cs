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

    /// <summary>戦闘の処理</summary>
    /// <param name="target">攻撃対象</param>
    /// <param name="attackpower">攻撃者</param>
    public void Battle(CharactorBase target, CharactorBase attackpower)
    {
        DamageCalculator(target, attackpower);
    }

    /// <summary>ダメージ計算をし、ダメージ結果を攻撃対象に渡す</summary>
    /// <param name="target">攻撃対象</param>
    /// <param name="attacker">攻撃者</param>
    public void DamageCalculator(CharactorBase target, CharactorBase attacker)
    {
        int totalDamage = 0;
        totalDamage += attacker.SendAtkPow();
        target.Damaged(totalDamage);
    }
}
