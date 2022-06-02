using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    private EnemyUIController() { }

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
    /// UIをカメラに対して正面に表示する
    /// </summary>
    private void Billboard()
    {
        var comPos = Camera.main.transform.position;
        comPos.y = transform.position.y;
        _canvas.transform.LookAt(comPos);
    }

    /// <summary>
    /// HPゲージの更新
    /// </summary>
    public void UpdateHPGauge(float currentHP, float maxHP)
    {
        if (_canvas.enabled == false)
        {
            _canvas.enabled = true;
        }
        _slider.value = currentHP / maxHP;
    }
}
