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
    /// HP�Q�[�W�̍X�V
    /// </summary>
    public void UpdateHPGauge(float current, float max)
    {
        var afterGauge = current / max;

        // 1/5�ȉ��̏ꍇ�͐F��ς���
        if (afterGauge <= 0.2f)
        {
            _fillImage.color = Color.red;
        }

        // �Q�[�W�̍X�V
        _hpGauge.value = afterGauge;
    }
    #endregion

    #region Private Function
    /// <summary>
    /// ������
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
