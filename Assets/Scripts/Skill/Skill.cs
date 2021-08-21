using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Skill")]
public class Skill : ScriptableObject
{
    public string SkillName => m_skillName;
    [SerializeField] string m_skillName = "";

    /// <summary>スキルの種類</summary>
    [SerializeField] public readonly SkillType m_skilltype = SkillType.Attack;

    /// <summary>スキルの属性</summary>
    [SerializeField] AttributeType m_attribute;

    /// <summary>スキルの攻撃力</summary>
    public int SkillPower => m_skillPower;
    [SerializeField] int m_skillPower = default;

    /// <summary>スキル使用時に必要なEP</summary>
    public int NeedEp => m_needEp;

    [SerializeField] int m_needEp;

    [SerializeField] GameObject m_effectPrefab;

    public enum SkillType
    {
        buff,
        Attack
    }
}
