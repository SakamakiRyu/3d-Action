using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mission : Singleton<Mission>
{
    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField] 
    int m_needDefeatCount = default;

    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField] 
    GameObject m_gameClearWindow = default;

    [Header("ゲームクリア時に表示する画像の背景")]
    [SerializeField] 
    Image m_clearBackGroundImage = default;

    [Header("敗北時に表示するパネル")]
    [SerializeField] 
    GameObject m_gameoverWindow = default;

    [SerializeField] 
    Image m_contollerImage = default;

    [SerializeField] 
    int m_currentDefeatCount = 0;

    /// <summary>ゲームクリアしているか</summary>
    public bool IsClear => m_currentDefeatCount >= m_needDefeatCount;

    public bool IsGameover { get; set; } = false;
    bool m_isColorChenge = false;

    /// <summary>ゲーム終了時に呼ばれるイベント</summary>
    public System.Action OnGameEnd;

    private void Update()
    {
        if (IsGameover)
        {
            OnGameEnd?.Invoke();
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
            OnGameEnd?.Invoke();
        }
    }

    /// <summary>
    /// ゲームスコアの加算
    /// </summary>
    public void GameScoreUp()
    {
        m_currentDefeatCount++;
    }

    IEnumerator TitleLoad()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}