using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [Header("ゲームクリアに必要な敵の討伐数")]
    [SerializeField]
    private int _needDefeatCount = default;

    [Header("ゲームクリア時に表示するパネル")]
    [SerializeField]
    private GameObject _gameClearWindow = default;

    [Header("ゲームクリア時に表示する画像の背景")]
    [SerializeField]
    private Image _clearBackGroundImage = default;

    [Header("敗北時に表示するパネル")]
    [SerializeField]
    private GameObject _gameoverWindow = default;

    [SerializeField]
    private Image _contollerImage = default;

    private int _currentDefeatCount = 0;

    /// <summary>ゲームクリアしているか</summary>
    public bool IsClear => _currentDefeatCount >= _needDefeatCount;

    private bool _isColorChenge = false;

    private void Start()
    {
        _currentDefeatCount = 0;
    }

    private void Update()
    {
        //if (IsGameover)
        //{
        //    OnGameEnd?.Invoke();
        //    m_contollerImage.enabled = false;
        //    StartCoroutine(TitleLoad());
        //    return;
        //}
    }

    /// <summary>
    /// ゲームスコアの加算
    /// </summary>
    public void AddScore()
    {
        _currentDefeatCount++;
        CheckGameClear();
    }

    public void GameEnd(PlayerParameter playerParam)
    {
        if (playerParam.CurrentState == PlayerParameter.State.Death)
        {
            _contollerImage.enabled = false;
            StartCoroutine(TitleLoad());
        }
    }

    private void CheckGameClear()
    {
        if (IsClear)
        {
            _gameClearWindow.SetActive(true);
            _contollerImage.enabled = false;
            if (!_isColorChenge)
            {
                _clearBackGroundImage.DOColor(new Color(1, 1, 1, 1), 4f);
            }
            StartCoroutine(TitleLoad());
            _isColorChenge = true;
        }
    }

    private IEnumerator TitleLoad()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}