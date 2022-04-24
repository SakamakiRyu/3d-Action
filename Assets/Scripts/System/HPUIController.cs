using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    [SerializeField] 
    Slider m_slider = default;

    [SerializeField]
    Canvas m_canvas = default;

    private void Start()
    {
        if (m_slider != null)
        {
            m_slider.value = 1.0f;
        }

        if (m_canvas != null)
        {
            m_canvas.enabled = false;
        }
    }

    public void Update()
    {
        Billboard();
    }
    
    /// <summary>
    /// UIのビルボード
    /// </summary>
    void Billboard()
    {
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }

    /// <summary>
    /// HPのアップデート
    /// </summary>
    public void UpdateHPSlider(EnemyController enemyDate)
    {
        if (m_canvas.enabled == false)
        {
            m_canvas.enabled = true;
        }
        m_slider.value = enemyDate.GetUIValue;
    }
}
