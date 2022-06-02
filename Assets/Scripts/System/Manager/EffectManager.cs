using System.Collections;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField]
    private GameObject[] Effects;

    public enum EffectType
    {
        /// <summary>ヒットエフェクト</summary>
        HitEffect = 0
    }

    /// <summary>
    /// ヒットストップをする
    /// </summary>
    public void PlayHitStop()
    {
        StartCoroutine(HitStopAcync());
    }

    /// <summary>
    /// ヒットエフェクトの再生
    /// </summary>
    /// <param name="generatePos"></param>
    public void PlayHitEffect(EffectType effectType, Vector3 generatePos)
    {
        Instantiate(Effects[(int)effectType], generatePos, Quaternion.identity);
    }

    /// <summary>
    /// ヒットストップ処理
    /// </summary>
    private IEnumerator HitStopAcync()
    {
        var _Timer = 0f;
        Time.timeScale = 0.1f;
        while (_Timer < 0.4f)
        {
            _Timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1.0f;
        yield return null;
    }
}
