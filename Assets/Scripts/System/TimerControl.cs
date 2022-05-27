using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// タイマーを操作するクラス
/// </summary>
public class TimerControl : MonoBehaviour
{
    private TimerControl() { }

    #region Field
    [SerializeField]
    private UnityEngine.UI.Text _timerText;

    private static float _timer = 0;
    public static float GetTimer
    {
        get
        {
            // 小数点第三位以下を切り捨てした値を受け取る
            return _timer;
        }
    }

    private static List<float> Times = new List<float>(5);
    public static List<float> GetTimers => Times;

    private bool _isCountup = false;
    #endregion

    #region Unity Func
    private void Start()
    {
        GameManager.Instance.OnEndInGame += StopTimer;
        ResetTimer();
    }

    private void Update()
    {
        CountUp();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnEndInGame -= StopTimer;
    }
    #endregion

    #region Public Func
    /// <summary>
    /// タイマーのリセット
    /// </summary>
    public void ResetTimer()
    {
        _timer = 0;
        ShowIngameText();
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

        if (MissionControl.IsMissionCleared == false)
        {
            // ミッションを失敗していた時はクリアタイムを999にする
            _timer = 999;
        }
        InsertTime(_timer);
    }
    #endregion

    #region Private Func
    /// <summary>
    /// カウントアップ
    /// </summary>
    private void CountUp()
    {
        if (_isCountup)
        {
            _timer += Time.deltaTime;
            ShowIngameText();
        }
    }

    /// <summary>
    /// インゲームのテキストにタイマーを反映させる
    /// </summary>
    private void ShowIngameText()
    {
        if (_isCountup)
        {
            var time = Math.Floor(_timer);
            _timerText.text = time.ToString();
        }
    }

    /// <summary>
    /// クリアタイムを受け取り、ランキングを更新する
    /// </summary>
    private void InsertTime(float clearTime)
    {
        if (clearTime == 999) return;

        // 少数第二位で切り捨て
        var time = Math.Floor(clearTime * Math.Pow(10, 2)) / Math.Pow(10, 2);

        Times.Add((float)time);

        // 降順ソート
        Times.OrderByDescending(x => x);
    }
    #endregion
}
