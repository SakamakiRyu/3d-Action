using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField, Header("キャラクターの種類")]
    public CharactorType m_type = CharactorType.None;

    [SerializeField, Header("攻撃判定のCollider")]
    Collider m_atkCollider = default;

    public bool m_IsAttack { get; set; } = false;

    public enum IsColliderEnabled { True, False }
    public void IsAttackCollider(IsColliderEnabled isAttack)
    {
        switch (isAttack)
        {
            case IsColliderEnabled.True:
                m_atkCollider.enabled = true;
                m_IsAttack = true;
                break;
            case IsColliderEnabled.False:
                m_atkCollider.enabled = false;
                m_IsAttack = false;
                break;
            default:
                break;
        }
    }

    public enum CharactorType
    {
        None,
        Player,
        Enemy
    }
}
