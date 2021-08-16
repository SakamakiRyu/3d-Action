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
        if (m_instance == null)
        {
            m_instance = this;
        }
    }

    /// <summary>
    /// ダメージ計算をし、ダメージ結果を攻撃対象に渡す
    /// </summary>
    /// <param name="targetStatus">攻撃対象のステータス</param>
    /// <param name="attacker">攻撃者のステータス</param>
    /// <returns>計算されたダメージ量</returns>
    public int DamageCalculator(StatusBase targetStatus, StatusBase attacker)
    {
        int totalDamage = 0;
        totalDamage += attacker.SendMyAttackPower();
        return totalDamage;
    }
}
