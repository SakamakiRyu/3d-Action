/// <summary>
/// 戦闘を管理するクラス
/// </summary>
public class BattleManager
{
    static BattleManager m_instance = new BattleManager();
    public static BattleManager Instance => m_instance;
    private BattleManager() { }    

    /// <summary>戦闘の処理</summary>
    /// <param name="targetDefPower">攻撃対象</param>
    /// <param name="attackpower">攻撃者</param>
    public void Battle(CharactorBase target, CharactorBase attacker)
    {
        int def = target.SendDefPower();
        int atk = attacker.SendAtkPower();
        int totalDamage = DamageCalculator(def, atk);
        target.Damaged(totalDamage);
    }

    /// <summary>ダメージ計算をし、ダメージ結果を攻撃対象に渡す</summary>
    /// <param name="target">攻撃を受ける側の防御力</param>
    /// <param name="attacker">攻撃する側の攻撃力</param>
    /// <returns>最終ダメージ</returns>
    public int DamageCalculator(int targetDefPower, int atkPower)
    {
        int totalDamage = 0;
        return totalDamage;
    }
}
