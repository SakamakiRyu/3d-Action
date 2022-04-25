using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("フェードにかける時間")]
    [SerializeField]
    private float _fadeTime = default;

    /// <summary>現在のシーン</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>ゲーム(InGame)終了時に呼ばれる処理</summary>
    public System.Action OnGameEnd;
    #endregion

    #region Unity Fucntion
    private void Awake()
    {
        ChengeScene(Scene.Title);
    }

    private void Start()
    {
    }

    private void Update()
    {
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
        ChengeScene(Scene.InGame);
    }
    #endregion

    #region Private Function
    /// <summary>
    /// シーンステートの変更をする
    /// </summary>
    private void ChengeScene(Scene next)
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

        CurrentScene = next;

        SceneLoad(next);
    }

    /// <summary>
    /// 指定したシーンのロードをする
    /// </summary>
    /// <param name="nextScene">ロードするシーン</param>
    private void SceneLoad(Scene nextScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)nextScene);
    }

    /// <summary>
    /// 画面フェードをする
    /// </summary>
    private IEnumerator DoFade(Image[] images, float time)
    {
        yield return null;
    }
    #endregion
}
