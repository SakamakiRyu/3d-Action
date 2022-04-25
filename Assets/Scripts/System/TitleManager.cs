using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    float _derayTime = default;

    Image[] _images = default;

    private void Start()
    {
        var images = GameObject.FindGameObjectsWithTag("SlimeSprites");
        _images = new Image[images.Length];
        int index = 0;
        foreach (var item in images)
        {
            _images[index] = item.GetComponent<Image>();
            index++;
        }
        _images = _images.OrderBy(i => System.Guid.NewGuid()).ToArray();
    }

    public void GameStart(string sceneName)
    {
        StartCoroutine(DoFade(_images, sceneName));
    }

    /// <summary>
    /// 画面フェード
    /// </summary>
    IEnumerator DoFade(Image[] images, string sceneName)
    {
        foreach (var item in images)
        {
            new WaitForSeconds(_derayTime);
            yield return item.color = new Color(1, 1, 1, 1);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
