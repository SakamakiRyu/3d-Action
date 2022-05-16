using System;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public Scene CurrentScene { get; private set; } = Scene.Title;
    /// <summary>ゲーム(InGame)終了時に呼ばれる処理</summary>
    public Action OnEndInGame;
    #endregion

    #region Unity Fucntion
    private void Start()
    {
        // シーン名
        var sceneName = CurrentScene.ToString();
        var currentSceneName = SceneManager.GetActiveScene().name;

        if (sceneName != currentSceneName)
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
                    LoadScene(sceneIndex);
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
    private void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // フェードアウト
        yield return FadeSystem.Instance.FadeOutAsync(1.5f);

        GC.Collect();

        var async = SceneManager.LoadSceneAsync(sceneIndex);

        async.allowSceneActivation = false;

        yield return null;

        // ロードが終わったらシーンを切り替える
        while (async.progress < 0.9f)
        {
            yield return null;
        }

        async.allowSceneActivation = true;

        yield return null;

        // フェードイン
        yield return FadeSystem.Instance.FadeInAsync(1.5f);

        yield return null;
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
