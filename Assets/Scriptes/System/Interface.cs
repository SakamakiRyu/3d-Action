/// <summary>
/// 攻撃(ダメージ)を受け取る事ができるようになる。
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public void AddDamage(int damage);
}