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
    [Header("�t�F�[�h�ɂ����鎞��")]
    [SerializeField]
    private float _fadeTime = default;

    /// <summary>���݂̃V�[��</summary>
    public Scene CurrentScene { get; private set; } = Scene.None;
    /// <summary>�Q�[��(InGame)�I�����ɌĂ΂�鏈��</summary>
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
        ChengeScene(Scene.InGame);
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
    /// �w�肵���V�[���̃��[�h������
    /// </summary>
    /// <param name="nextScene">���[�h����V�[��</param>
    private void SceneLoad(Scene nextScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)nextScene);
    }

    /// <summary>
    /// ��ʃt�F�[�h������
    /// </summary>
    private IEnumerator DoFade(Image[] images, float time)
    {
        yield return null;
    }
    #endregion
}
