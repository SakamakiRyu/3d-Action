using UnityEngine;

/// <summary>
/// 攻撃処理クラス
/// ※ 攻撃はColliderを使っての当たり判定でする為、攻撃するGameObjectは
///    武器などにColliderをつける事。
///    また、攻撃を受け取る側はIDamageableインターフェースを実装する事。
/// </summary>
public class AttackBehaviour : MonoBehaviour
{
    [SerializeField]
    private int _AttackPower;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable go;
        other.TryGetComponent<IDamageable>(out go);

        if (go is not null)
        {
            // 後ほど攻撃力を設定
            go.AddDamage(_AttackPower);
        }
    }
}