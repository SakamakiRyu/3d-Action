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
        other.TryGetComponent(out IDamageable go);

        if (go != null)
        {
            go.AddDamage();
        }
    }
}