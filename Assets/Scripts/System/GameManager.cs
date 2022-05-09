public class GameManager : Singleton<GameManager>
{
    #region Define
    public enum Scene
    {
        None = -1,
        Title,
        InGame,
        Result
    }
    #endregion

    #region Property
    /// <summary>現在のシーン</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>ゲーム(InGame)終了時に呼ばれる処理</summary>
    public System.Action OnGameEnd;
    #endregion

    #region Unity Fucntion
    private void Start()
    {
        if (CurrentScene == Scene.None)
        {
            ChengeSceneState(Scene.Title);
        }
    }

    private void Update()
    {
        UpdateState();
    }
    #endregion

    #region Public Fucntion
    /// <summary>
    /// ゲーム終了の通知を受け取る
    /// </summary>
    public void RequestGameEnd()
    {
        OnGameEnd?.Invoke();
    }

    /// <summary>
    /// タイトルのButton(UI)用
    /// </summary>
    /// <param name="scene"></param>
    public void StartButton()
    {
        ChengeSceneState(Scene.InGame);
    }
    #endregion

    #region Private Function
    /// <summary>
    /// ステート毎に毎フレーム呼ばれる処理
    /// </summary>
    private void UpdateState()
    {
        switch (CurrentScene)
        {
            case Scene.Title:
                {
                }
                break;
            case Scene.InGame:
                break;
            case Scene.Result:
                break;
        }
    }

    /// <summary>
    /// シーンステートの変更をする
    /// </summary>
    private void ChengeSceneState(Scene next)
    {
        switch (next)
        {
            case Scene.Title:
                {
                    SoundManager.Instance.ChengeBGM(SoundManager.BGMType.Title);
                }
                break;
            case Scene.InGame:
                {
                    SoundManager.Instance.ChengeBGM(SoundManager.BGMType.InGame);
                }
                break;
            case Scene.Result:
                {
                    OnGameEnd?.Invoke();
                }
                break;
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)next);

        CurrentScene = next;
    }
    #endregion
}
