using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class DisplayHp : MonoBehaviour
{
    [SerializeField, Tooltip("HPを表示するSlider(UI)を設定")]
    Slider m_hpSlider;
    [Space]

    [SerializeField]
    Player m_player = null;
    [Space]

    [SerializeField]
    Enemy m_enemy = null;

    [SerializeField, Tooltip("DoTweenアニメーションの再生時間")]
    float m_animTime = 2f;

    public void OnReflect()
    {
        if (m_player)
        {
            DOTween.To(() => m_hpSlider.value, x => m_hpSlider.value = x, (float)m_player.CurrentHP / m_player.MaxHP, m_animTime).SetEase(Ease.Unset);
        }
        else if (m_enemy)
        {
            DOTween.To(() => m_hpSlider.value, x => m_hpSlider.value = x, (float)m_player.CurrentHP / m_player.MaxHP, m_animTime).SetEase(Ease.Unset);
        }
    }

    private void OnValidate()
    {
        Check();
    }

    void Check()
    {
        if (m_player && m_enemy)
        {
            Debug.LogError("PlayerとEnemyを同時に設定することはできません。");
            m_player = null;
            m_enemy = null;
        }
    }
}
