using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private PlayerUIController() { }

    #region Field
    [SerializeField]
    private Slider _hpGauge = default;
    #endregion

    #region Public Fucntion
    /// <summary>
    /// HPゲージの更新
    /// </summary>
    public void UpdateHPGauge(float current, float max)
    {
        _hpGauge.value = current / max;
    }
    #endregion
}
