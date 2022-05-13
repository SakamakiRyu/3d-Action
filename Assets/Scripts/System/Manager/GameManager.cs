using System.Collections;
using UnityEngine;

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
    [Header("タイトル遷移時にかかる遅延時間")]
    [SerializeField]
    private float _delayTimeOfTitleLoad;
    /// <summary>現在のシーン</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>ゲーム(InGame)終了時に呼ばれる処理</summary>
    public System.Action OnEndInGame;
    #endregion

    #region Unity Fucntion
    private void Start()
    {
        if (CurrentScene == Scene.None)
        {
            ChengeSceneState(Scene.Title);
        }
        OnEndInGame += GameEnd;
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
        OnEndInGame?.Invoke();
    }

    /// <summary>
    /// InGameSceneのロードをする
    /// </summary>
    /// <param name="scene"></param>
    public void LoadToInGameScene()
    {
        ChengeSceneState(Scene.InGame);
    }

    /// <summary>
    /// TitleSceneのロードをする
    /// </summary>
    public void LoadToTitleScene()
    {
        ChengeSceneState(Scene.Title);
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
        var sceneIndex = (int)next;

        switch (next)
        {
            case Scene.Title:
                {
                    LoadScene(sceneIndex);
                    SoundManager.Instance.ChengeBGM(SoundManager.BGMType.Title);
                }
                break;
            case Scene.InGame:
                {
                    LoadScene(sceneIndex);
                    SoundManager.Instance.ChengeBGM(SoundManager.BGMType.InGame);
                }
                break;
            case Scene.Result:
                {
                    LoadScene(sceneIndex, _delayTimeOfTitleLoad);
                    SoundManager.Instance.ChengeBGM(SoundManager.BGMType.Title);
                }
                break;
        }

        CurrentScene = next;
    }

    /// <summary>
    /// シーンのロードをする
    /// </summary>
    /// <param name="sceneIndex">SceneIndex</param>
    /// <param name="delayTime">シーン遷移にかける時間</param>
    private void LoadScene(int sceneIndex, float delayTime = 0f)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex, delayTime));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
    }

    /// <summary>
    /// OnGameEndに呼ばれる関数
    /// </summary>
    private void GameEnd()
    {
        ChengeSceneState(Scene.Result);
    }
    #endregion
}
