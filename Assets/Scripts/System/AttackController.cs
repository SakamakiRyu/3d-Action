using UnityEngine;

/// <summary>
/// 攻撃処理クラス
/// ※ 攻撃はColliderを使っての当たり判定でする為、攻撃するGameObjectは
///    武器などにColliderをつける事。
///    また、攻撃を受け取る側はIDamageableインターフェースを実装する事。
/// </summary>
public class AttackController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out IDamageable damageable);
        other.TryGetComponent(out EnemyController enemy);

        if (damageable != null)
        {
            damageable.AddDamage();
        }

        // プレイヤーの攻撃が当たった相手が敵だった場合の処理
        if (enemy != null)
        {
            // エフェクトの再生
            EffectManager.Instance.PlayHitEffect(EffectManager.EffectType.HitEffect, other.ClosestPointOnBounds(this.transform.position));
        }
    }
}