/// <summary>
/// 戦闘を管理するクラス
/// </summary>
public static class BattleHelper
{
    /// <summary>戦闘の処理</summary>
    /// <param name="targetDefPower">攻撃対象</param>
    /// <param name="attackpower">攻撃者</param>
    public static void Battle(CharactorBase target, CharactorBase attacker)
    {
        int def = target.SendDefPower();
        int atk = attacker.SendAtkPower();
        int totalDamage = DamageCalculator(def, atk);
        target.Damaged(totalDamage);
    }

    /// <summary>ダメージ計算をし、ダメージ結果を攻撃対象に渡す</summary>
    /// <param name="target">攻撃を受ける側の防御力</param>
    /// <param name="attacker">攻撃する側の攻撃力</param>
    /// <returns>最終ダメージ(1以下のダメージは1固定)</returns>
    static int DamageCalculator(int targetDefPower, int atkPower)
    {
        int totalDamage = atkPower - targetDefPower;
        if (totalDamage < 1)
        {
            totalDamage = 1;
        }
        return totalDamage;
    }

    /// <summary>弱点だった場合の処理を追加</summary>
    static void Weaknesses()
    {

    }
}
