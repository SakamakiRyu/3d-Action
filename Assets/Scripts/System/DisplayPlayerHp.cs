using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class DisplayPlayerHp : MonoBehaviour
{
    [SerializeField, Tooltip("HPを表示するSlider(UI)を設定")]
    Slider m_hpSlider;
    [Space]

    [SerializeField]
    Player m_player = null;
    [Space]

    [SerializeField, Tooltip("何秒かけてスライダーゲージを減らすか。")]
    float m_animTime = default;

    private void Start()
    {
        if (m_hpSlider)
        {
            m_hpSlider.value = 1;
            m_player.m_damaged += OnReflect;
        }
        else
        {
            Debug.LogError("HpSliderがアサインされていません");
            m_player = GameObject.Find("Player").GetComponent<Player>();
            m_hpSlider.value = 1;
            m_player.m_damaged += OnReflect;
        }
    }

    public void OnReflect()
    {
        if (m_player)
        {
            DOTween.To(() => m_hpSlider.value, x => m_hpSlider.value = x, (float)m_player.CurrentHP / m_player.MaxHP, m_animTime).SetEase(Ease.Unset);
        }
    }
}