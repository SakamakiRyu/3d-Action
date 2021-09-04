using UnityEngine;

public class UseSkill : MonoBehaviour
{
    [SerializeField] Skill m_skill;

    public void Skill()
    {
        if (m_skill.m_skilltype ==  global::Skill.SkillType.Attack)
        {

        }
    }
}
