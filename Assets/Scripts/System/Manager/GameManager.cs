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
    [Header("�^�C�g���J�ڎ��ɂ�����x������")]
    [SerializeField]
    private float _delayTimeOfTitleLoad;
    /// <summary>���݂̃V�[��</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>�Q�[��(InGame)�I�����ɌĂ΂�鏈��</summary>
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
                    LoadScene(sceneIndex, _delayTimeOfTitleLoad);
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
    /// OnGameEnd�ɌĂ΂��֐�
    /// </summary>
    private void GameEnd()
    {
        ChengeSceneState(Scene.Result);
    }
    #endregion
}
