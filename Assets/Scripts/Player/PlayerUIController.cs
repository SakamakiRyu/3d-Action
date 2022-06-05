using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private PlayerUIController() { }

    #region Field
    [SerializeField]
    private Slider _hpGauge = default;

    [SerializeField]
    private Image _fillImage;
    #endregion

    #region Unity Function
    private void Start()
    {
        Init();
    }
    #endregion

    #region Public Fucntion
    /// <summary>
    /// HPゲージの更新
    /// </summary>
    public void UpdateHPGauge(float current, float max)
    {
        var afterGauge = current / max;

        // 1/5以下の場合は色を変える
        if (afterGauge <= 0.2f)
        {
            _fillImage.color = Color.red;
        }

        // ゲージの更新
        _hpGauge.value = afterGauge;
    }
    #endregion

    #region Private Function
    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        if (_hpGauge)
        {
            _hpGauge.value = 1.0f;
        }

        if (_fillImage)
        {
            _fillImage.color = Color.green;
        }
    }
    #endregion
}
