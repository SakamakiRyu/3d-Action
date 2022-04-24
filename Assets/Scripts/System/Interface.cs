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

/// <summary>
/// ゲームクリア時に処理を
/// </summary>
interface IGame
{
    /// <summary>イベントに登録する関数</summary>
    void Register();
    /// <summary>ゲームクリア時に呼ばれる関数</summary>
    void GameClear();
}