using System;
using UnityEngine;

/// <summary>
/// �^�C�}�[�𑀍삷��N���X
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
    /// �e�L�X�g�Ƀ^�C�}�[�𔽉f������
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
    /// �^�C�}�[�̃��Z�b�g
    /// </summary>
    public void ResetTimer()
    {
        _timer = 0;
        ShowText();
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
    }
}
