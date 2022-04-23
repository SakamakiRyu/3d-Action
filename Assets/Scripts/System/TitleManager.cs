using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TitleManager : MonoBehaviour
{
    [SerializeField] float m_derayTime = default;
    [SerializeField] AudioClip m_pressSE = default;
    Image[] m_images = default;
    AudioSource m_source = default;

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        var images = GameObject.FindGameObjectsWithTag("SlimeSprites");
        m_images = new Image[images.Length];
        int index = 0;
        foreach (var item in images)
        {
            m_images[index] = item.GetComponent<Image>();
            index++;
        }
        m_images = m_images.OrderBy(i => System.Guid.NewGuid()).ToArray();
    }

    public void GameStart(string sceneName)
    {
        m_source.PlayOneShot(m_pressSE);
        StartCoroutine(Fade(m_images, sceneName));
    }

    IEnumerator Fade(Image[] images, string sceneName)
    {
        foreach (var item in images)
        {
            new WaitForSeconds(m_derayTime);
            yield return item.color = new Color(1, 1, 1, 1);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
