using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Breed")]
public class Breed : ScriptableObject
{
    /// <summary>親系統</summary>
    [SerializeField] Breed m_parent = null;

    /// <summary>敵のアイコン</summary>
    [SerializeField] Sprite m_sprite = default;

    public Sprite Sprite => m_sprite;

    [SerializeField] string m_name = "";
    /// <summary>系統名</summary>
    public string Name
    {
        get
        {
            if (m_parent)
            {
                return m_parent.name;
            }
            return m_name;
        }
    }

    [SerializeField] AttributeType m_weaknesses = AttributeType.None;

    /// <summary>弱点属性</summary>
    public AttributeType Weaknesses
    {
        get
        {
            if (m_IsOverrideWeaknesses || !m_parent)
            {
                return m_weaknesses;
            }
            return m_parent.Weaknesses;
        }
    }

    [SerializeField] bool m_IsOverrideWeaknesses = false;

    [SerializeField] int m_maxHp = default;
    public int MaxHp => m_maxHp;

    [SerializeField] int m_atkPower = default;
    public int AtkPower => m_atkPower;

    [SerializeField] bool m_IsOverrideMaxHp = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        ValidateParent();
    }

    public void ValidateParent()
    {
        if (!m_parent) return;

        Breed currnt = m_parent;
        while (currnt)
        {
            if (currnt == this)
            {
                m_parent = null;
                break;
            }
            currnt = currnt.m_parent;
        }
    }
#endif
}