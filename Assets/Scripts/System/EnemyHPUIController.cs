using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUIController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider = default;

    [SerializeField]
    private Canvas _canvas = default;

    private void Start()
    {
        if (_slider != null)
        {
            _slider.value = 1.0f;
        }

        if (_canvas != null)
        {
            _canvas.enabled = false;
        }
    }

    public void Update()
    {
        Billboard();
    }

    /// <summary>
    /// UI���J�����ɑ΂��Đ��ʂɕ\������
    /// </summary>
    private void Billboard()
    {
        var comPos = Camera.main.transform.position;
        comPos.y = transform.position.y;
        _canvas.transform.LookAt(comPos);
    }

    /// <summary>
    /// �X���C�_�[�\���̍X�V
    /// </summary>
    public void UpdateHPSlider(SlimeController enemyDate)
    {
        if (_canvas.enabled == false)
        {
            _canvas.enabled = true;
        }
        _slider.value = enemyDate.GetUIValue;
    }
}
