﻿using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

interface IGameEnd
{
    /// <summary>イベントに登録する関数</summary>
    void Register();
    /// <summary>ゲームクリア時に呼ばれる関数</summary>
    void OnEnd();
}

public class Mission : MonoBehaviour
{
    public static Mission Instance => m_instance;
    static Mission m_instance;
    private Mission() { }

    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField] int m_needDefeatCount = default;
    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField] GameObject m_gameClearWindow = default;
    [Header("ゲームクリア時に表示する画像の背景")]
    [SerializeField] Image m_clearBackGroundImage = default;
    [Header("敗北時に表示するパネル")]
    [SerializeField] GameObject m_gameoverWindow = default;
    [SerializeField] Image m_contollerImage = default;
    /// <summary>現在の討伐数</summary>
    [SerializeField] int m_currentDefeatCount = 0;
    /// <summary>ゲームクリアしているか</summary>
    public bool IsClear => m_currentDefeatCount >= m_needDefeatCount;
    /// <summary>ゲーム終了時に呼ばれるイベント</summary>
    public System.Action GameEnd;

    public bool IsGameover { get; set; } = false;
    bool m_isColorChenge = false;
 

    private void Awake()
    {
        m_instance = this;
    }

    private void Update()
    {
        if (IsGameover)
        {
            GameEnd?.Invoke();
            m_contollerImage.enabled = false;
            StartCoroutine(TitleLoad());
            return;
        }

        if (IsClear)
        {
            m_gameClearWindow.SetActive(true);
            m_contollerImage.enabled = false;
            if (!m_isColorChenge)
            {
                m_clearBackGroundImage.DOColor(new Color(1, 1, 1, 1), 4f);
            }
            StartCoroutine(TitleLoad());
            m_isColorChenge = true;
            GameEnd?.Invoke();
        }
    }

    public void GameUpdate()
    {
        m_currentDefeatCount++;
    }

    IEnumerator TitleLoad()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}