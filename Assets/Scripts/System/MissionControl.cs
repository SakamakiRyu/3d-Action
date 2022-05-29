using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// ミッション
/// </summary>
public class MissionControl : MonoBehaviour
{
    #region Field
    public static bool IsMissionCleared { get; private set; } = false;

    [SerializeField]
    private PlayerInput _pInput = default;

    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField]
    private GameObject _gameClearWindow = default;

    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField]
    private int _needDefeatCount = default;

    [Header("討伐数を管理する")]
    [Tooltip("画像をHorizontalLayoutGroupで並べる為。")]
    [SerializeField]
    private HorizontalLayoutGroup _parent = default;

    [Header("並べる画像(Prefab)")]
    [SerializeField]
    private GameObject _missionSprite = default;

    private GameObject[] _images = default;
    /// <summary>現在の討伐数</summary>
    private int _currentDefeatCount = 0;
    /// <summary>ポーズ中か</summary>
    private bool IsPaused = false;
    /// <summary>Ingame前のカットシーンが再生済みか</summary>
    public bool IsPlayedMovie { get; private set; } = false;
    /// <summary>InputSystemの入力を受け取る</summary>
    private InputAction _pause = default;
    #endregion

    #region Unity Function
    private void Awake()
    {
    }

    private void Start()
    {
        Initialized();
    }

    private void Update()
    {
        Pause();
    }
    #endregion

    #region Private Fucntion
    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialized()
    {
        _currentDefeatCount = 0;
        IsPlayedMovie = false;
        IsMissionCleared = false;
        _pause = _pInput.currentActionMap["Pause"];
        GenerateImages(_needDefeatCount);
    }

    /// <summary>
    /// 討伐数を表す画像を生成する
    /// </summary>
    private void GenerateImages(int count)
    {
        _images = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            _images[i] = Instantiate(_missionSprite, _parent.transform).GetComponent<GameObject>();
        }
    }

    /// <summary>
    /// ポーズの切り替え処理
    /// <para>一時停止</para>
    /// </summary>
    private void Pause()
    {
        if (_pause == null) return;

        if (_pause.triggered)
        {
            switch (IsPaused)
            {
                case true:
                    {
                        Time.timeScale = 1.0f;
                        IsPaused = false;
                    }
                    break;
                case false:
                    {
                        Time.timeScale = 0f;
                        IsPaused = true;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// ゲームをクリアしたかの判定
    /// <para>クリアしていたら処理を行う</para>
    /// </summary>
    private bool IsGameClear()
    {
        // 必要討伐数に到達したらミッションクリアとみなす
        var isClear = _currentDefeatCount >= _needDefeatCount;

        if (isClear)
        {
            IsMissionCleared = true;
            _gameClearWindow.SetActive(true);
            GameManager.Instance.GameEndRequest();
            return true;
        }

        return false;
    }
    #endregion

    #region Public Function
    /// <summary>
    /// ゲームスコアの加算
    /// </summary>
    public void AddScore()
    {
        _currentDefeatCount++;
        // 現在の討伐数
        var index = _currentDefeatCount;
        // 討伐数を表す画像を非表示にする
        _images[index - 1].SetActive(false);
        
        IsGameClear();
    }

    /// <summary>
    /// カットシーンの再生が終わったら呼ばれる
    /// </summary>
    public void OnEndedMovie(bool frag)
    {
        IsPlayedMovie = frag;
    }
    #endregion
}