using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

interface IGameClear
{
    /// <summary>イベントに登録する関数</summary>
    void Register();
    /// <summary>ゲームクリア時に呼ばれる関数</summary>
    void OnClear();
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance => m_instance;
    static QuestManager m_instance;
    private QuestManager() { }

    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField] int m_needDefeatCount = default;
    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField] GameObject m_gameClearWindow = default;
    [Header("ゲームクリア時に表示する画像の背景")]
    [SerializeField] Image m_clearBackGroundImage = default;
    /// <summary>現在の討伐数</summary>
    [SerializeField] int m_currentDefeatCount = 0;
    /// <summary>ゲームクリアしているか</summary>
    public bool IsClear => m_currentDefeatCount >= m_needDefeatCount;
    /// <summary>ゲームクリア時に呼ばれるイベント</summary>
    public System.Action GameClear;

    private void Awake()
    {
        m_instance = this;
    }

    private void Update()
    {
        if (IsClear)
        {
            m_gameClearWindow.SetActive(true);
            m_clearBackGroundImage.DOColor(new Color(1, 1, 1, 1), 5f);
            GameClear?.Invoke();
        }
    }

    public void GameUpdate()
    {
        m_currentDefeatCount++;
    }
}