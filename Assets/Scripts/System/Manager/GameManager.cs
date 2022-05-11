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
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>�Q�[��(InGame)�I�����ɌĂ΂�鏈��</summary>
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
    /// �Q�[���I���̒ʒm���󂯎��
    /// </summary>
    public void RequestGameEnd()
    {
        OnGameEnd?.Invoke();
    }

    /// <summary>
    /// �^�C�g����Button(UI)�p
    /// </summary>
    /// <param name="scene"></param>
    public void StartButton()
    {
        ChengeSceneState(Scene.InGame);
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
    /// �V�[���X�e�[�g�̕ύX������
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
