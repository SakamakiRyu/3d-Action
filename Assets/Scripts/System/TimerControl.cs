using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �^�C�}�[�𑀍삷��N���X
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
            // �����_��O�ʈȉ���؂�̂Ă����l���󂯎��
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
    /// �^�C�}�[�̃��Z�b�g
    /// </summary>
    public void ResetTimer()
    {
        _timer = 0;
        ShowIngameText();
    }

    /// <summary>
    /// �^�C�}�[�̃X�^�[�g
    /// </summary>
    public void StartTimer()
    {
        _isCountup = true;
    }

    /// <summary>
    /// �^�C�}�[���~�߂�
    /// </summary>
    public void StopTimer()
    {
        _isCountup = false;

        if (MissionControl.IsMissionCleared == false)
        {
            // �~�b�V���������s���Ă������̓N���A�^�C����999�ɂ���
            _timer = 999;
        }
        InsertTime(_timer);
    }
    #endregion

    #region Private Func
    /// <summary>
    /// �J�E���g�A�b�v
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
    /// �C���Q�[���̃e�L�X�g�Ƀ^�C�}�[�𔽉f������
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
    /// �N���A�^�C�����󂯎��A�����L���O���X�V����
    /// </summary>
    private void InsertTime(float clearTime)
    {
        if (clearTime == 999) return;

        // �������ʂŐ؂�̂�
        var time = Math.Floor(clearTime * Math.Pow(10, 2)) / Math.Pow(10, 2);

        Times.Add((float)time);

        // �~���\�[�g
        Times.OrderByDescending(x => x);
    }
    #endregion
}
