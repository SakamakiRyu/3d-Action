using UnityEngine;

public class MissionControl : MonoBehaviour
{
    private MissionControl() { }

    public static bool IsCleared { get; private set; } = false;

    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField]
    private int _needDefeatCount = default;

    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField]
    private GameObject _gameClearWindow = default;

    [SerializeField]
    private UnityEngine.UI.Image[] _images;

    private int _currentDefeatCount = 0;

    private void Awake()
    {
        _currentDefeatCount = 0;
    }

    private void Start()
    {
        IsCleared = false;
    }

    /// <summary>
    /// ゲームスコアの加算
    /// </summary>
    public void AddScore()
    {
        var index = _currentDefeatCount;
        _images[index].enabled = false;
        _currentDefeatCount++;
        IsGameClear();
    }

    /// <summary>
    /// ゲームをクリアしたかの判定
    /// クリアしていたら処理を行う
    /// </summary>
    private bool IsGameClear()
    {
        // 必要討伐数に到達したらミッションクリアとみなす
        var isClear = _currentDefeatCount >= _needDefeatCount;

        if (isClear)
        {
            IsCleared = true;
            _gameClearWindow.SetActive(true);
            GameManager.Instance.RequestGameEnd();
            return true;
        }

        return false;
    }
}