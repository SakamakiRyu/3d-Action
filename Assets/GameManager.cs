using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private Mission _mission;

    /// <summary>現在のシーン</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>ゲーム(InGame)開始時に呼ばれる処理</summary>
    public System.Action OnGameStart;
    /// <summary>ゲーム(InGame)終了時に呼ばれる処理</summary>
    public System.Action OnGameEnd;
    #endregion

    #region Unity Fucntion
    private void Awake()
    {
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
    /// ゲームの終了通知を受け取る
    /// </summary>
    public void RequestGameEnd()
    {
        _mission.OnGameEnd();
    }

    /// <summary>
    /// Button(UI)用の関数
    /// </summary>
    /// <param name="scene"></param>
    public void LoadButton(Scene scene)
    {
        ChengeScene(scene);
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
                { }
                break;
            case Scene.InGame:
                {
                    OnGameStart?.Invoke();
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
    #endregion
}
