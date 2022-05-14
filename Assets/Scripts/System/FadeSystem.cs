using System;
using System.Collections;
using UnityEngine;

public class FadeSystem : Singleton<FadeSystem>
{
    [SerializeField]
    private UnityEngine.UI.Image _image;

    public IEnumerator FadeInAsync(float delayTime)
    {
        float timer = delayTime;
        var alfa = _image.color;
        alfa.a = 1f;
        _image.color = alfa;

        yield return null;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            alfa.a = timer / delayTime;
            _image.color = alfa;
            yield return null;
        }

        yield return null;
    }

    public IEnumerator FadeOutAsync(float detayTime)
    {
        float timer = 0;
        var alfa = _image.color;
        alfa.a = 0;
        _image.color = alfa;

        yield return null;

        while (timer < detayTime)
        {
            timer += Time.deltaTime;
            alfa.a = timer / detayTime;
            _image.color = alfa;
            yield return null;
        }

        yield return null;
    }
}
