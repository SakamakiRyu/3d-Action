using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class DisplayPlayerStatus : MonoBehaviour
{
    [SerializeField, Tooltip("HPを表示するSlider(UI)を設定")]
    Slider m_hpSlider = default;

    [SerializeField, Tooltip("EPを表示するSlider(UI)を設定")]
    Slider m_epSlider = default;

    [SerializeField]
    Player m_player = default;

    [SerializeField, Tooltip("何秒かけてスライダーゲージを減らすか。")]
    float m_animTime = default;

    private void Start()
    {
        if (!m_epSlider || !m_hpSlider || !m_player)
        {
            Debug.LogError($"{this.gameObject.name}に参照が足りていません");
            return;
        }
        m_hpSlider.value = 1; m_epSlider.value = 1;
        m_player.Damaged += OnReflectHP;
        m_player.UseSkill += OnReflectEP;
    }

    private void OnDestroy()
    {
        m_player.Damaged -= OnReflectHP;
        m_player.UseSkill -= OnReflectEP;
    }

    public void OnReflectHP()
    {
        if (m_player)
        {
            DOTween.To(() => m_hpSlider.value, x => m_hpSlider.value = x, (float)m_player.CurrentHP / m_player.MaxHP, m_animTime).SetEase(Ease.Unset);
        }
    }

    public void OnReflectEP()
    {
        if (m_player)
        {
            DOTween.To(() => m_epSlider.value, x => m_epSlider.value = x, (float)m_player.CurrentEP / m_player.MaxEP, m_animTime).SetEase(Ease.Unset);
        }
    }
}