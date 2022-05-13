﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MissionControl : MonoBehaviour
{
    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField]
    private int _needDefeatCount = default;

    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField]
    private GameObject _gameClearWindow = default;

    [Header("ゲームクリア時に表示する画像の背景")]
    [SerializeField]
    private Image _clearBackGroundImage = default;

    [Header("敗北時に表示するパネル")]
    [SerializeField]
    private GameObject _gameoverWindow = default;

    private int _currentDefeatCount = 0;

    private void Awake()
    {
        _currentDefeatCount = 0;
    }

    private void Start()
    {

    }

    private void Update()
    {
    }

    /// <summary>
    /// ゲームスコアの加算
    /// </summary>
    public void AddScore()
    {
#if UNITY_EDITOR
        Debug.Log("スコア加算");
#endif
        _currentDefeatCount++;
        IsGameClear();
    }

    /// <summary>
    /// ゲームをクリアしたか
    /// </summary>
    private void IsGameClear()
    {
        // 必要討伐数に到達したらミッションクリアとみなす
        var isClear = _currentDefeatCount >= _needDefeatCount;

        if (isClear)
        {
            _gameClearWindow.SetActive(true);
            GameManager.Instance.RequestGameEnd();
        }
    }
}