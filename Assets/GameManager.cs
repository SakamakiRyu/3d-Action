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

    /// <summary>���݂̃V�[��</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>�Q�[��(InGame)�J�n���ɌĂ΂�鏈��</summary>
    public System.Action OnGameStart;
    /// <summary>�Q�[��(InGame)�I�����ɌĂ΂�鏈��</summary>
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
    /// �Q�[���̏I���ʒm���󂯎��
    /// </summary>
    public void RequestGameEnd()
    {
        _mission.OnGameEnd();
    }

    /// <summary>
    /// Button(UI)�p�̊֐�
    /// </summary>
    /// <param name="scene"></param>
    public void LoadButton(Scene scene)
    {
        ChengeScene(scene);
    }
    #endregion

    #region Private Function
    /// <summary>
    /// �V�[���X�e�[�g�̕ύX������
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
    /// �w�肵���V�[���̃��[�h������
    /// </summary>
    /// <param name="nextScene">���[�h����V�[��</param>
    private void SceneLoad(Scene nextScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)nextScene);
    }
    #endregion
}
