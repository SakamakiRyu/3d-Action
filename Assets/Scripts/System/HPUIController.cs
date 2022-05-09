using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
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
    /// UIをカメラに対して正面に表示する
    /// </summary>
    private void Billboard()
    {
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }

    /// <summary>
    /// スライダー表示の更新
    /// </summary>
    public void UpdateHPSlider(EnemyController enemyDate)
    {
        if (_canvas.enabled == false)
        {
            _canvas.enabled = true;
        }
        _slider.value = enemyDate.GetUIValue;
    }
}
