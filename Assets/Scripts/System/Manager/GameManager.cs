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
    /// <summary>���݂̃V�[��</summary>
    public Scene CurrentScene { get; private set; } = Scene.Title;
    /// <summary>�Q�[��(InGame)�I�����ɌĂ΂�鏈��</summary>
    public Action OnEndInGame;
    #endregion

    #region Unity Fucntion
    private void Start()
    {
        // �V�[����
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
    /// �Q�[���I���̒ʒm���󂯎��
    /// </summary>
    public void RequestGameEnd()
    {
        OnEndInGame?.Invoke();
    }

    /// <summary>
    /// InGameScene�̃��[�h������
    /// </summary>
    /// <param name="scene"></param>
    public void LoadToInGameScene()
    {
        ChengeSceneState(Scene.InGame);
    }

    /// <summary>
    /// TitleScene�̃��[�h������
    /// </summary>
    public void LoadToTitleScene()
    {
        ChengeSceneState(Scene.Title);
    }
    #endregion

    #region Private Function
    /// <summary>
    /// �X�e�[�g���ɖ��t���[���Ă΂�鏈��
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
    /// �V�[���X�e�[�g�̕ύX������
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
    /// �V�[���̃��[�h������
    /// </summary>
    /// <param name="sceneIndex">SceneIndex</param>
    /// <param name="delayTime">�V�[���J�ڂɂ����鎞��</param>
    private void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // �t�F�[�h�A�E�g
        yield return FadeSystem.Instance.FadeOutAsync(1.5f);

        GC.Collect();

        var async = SceneManager.LoadSceneAsync(sceneIndex);

        async.allowSceneActivation = false;

        yield return null;

        // ���[�h���I�������V�[����؂�ւ���
        while (async.progress < 0.9f)
        {
            yield return null;
        }

        async.allowSceneActivation = true;

        yield return null;

        // �t�F�[�h�C��
        yield return FadeSystem.Instance.FadeInAsync(1.5f);

        yield return null;
    }

    /// <summary>
    /// OnGameEnd�ɌĂ΂��֐�
    /// </summary>
    private void GameEnd()
    {
        ChengeSceneState(Scene.Result);
    }
    #endregion
}
