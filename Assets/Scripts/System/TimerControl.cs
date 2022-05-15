using System;
using UnityEngine;

/// <summary>
/// タイマーを操作するクラス
/// </summary>
public class TimerControl : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text _timerText;

    private float _timer;
    public float GetTimer => _timer;

    private bool _isCountup = false;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        CountUp();
    }

    private void CountUp()
    {
        if (_isCountup)
        {
            _timer += Time.deltaTime;
            ShowText();
        }
    }

    /// <summary>
    /// テキストにタイマーを反映させる
    /// </summary>
    private void ShowText()
    {
        if (_isCountup)
        {
            var time = Math.Floor(_timer);
            _timerText.text = time.ToString();
        }
    }

    /// <summary>
    /// タイマーのリセット
    /// </summary>
    public void ResetTimer()
    {
        _timer = 0;
        ShowText();
    }

    /// <summary>
    /// タイマーのスタート
    /// </summary>
    public void StartTimer()
    {
        _isCountup = true;
    }

    /// <summary>
    /// タイマーを止める
    /// </summary>
    public void StopTimer()
    {
        _isCountup = false;
    }
}
